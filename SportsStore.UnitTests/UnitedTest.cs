using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HTMLHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitedTest
    {
        Mock<IProductRepository> simulateRepository = new Mock<IProductRepository>();
        Product prod1, prod2;
        public UnitedTest()
        {
            simulateRepository.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 0, Name = "P1",Category="Cat1"},
                new Product {ProductId = 1, Name = "P2",Category="Cat2"},
                new Product {ProductId = 2, Name = "P3",Category="Cat1"},
                new Product {ProductId = 3, Name = "P4",Category="Cat2"},
                new Product {ProductId = 4, Name = "P5",Category="Cat3"},
            }.AsQueryable());
            prod1 = new Product
            {
                ProductId = 0,
                Name = "P1",
                Price = 20m,
                Category = "Cat1"
            };
            prod2 = new Product
            {
                ProductId = 1,
                Name = "P2",
                Price = 12.34m,
                Category = "Cat2"
            };

        }
        [TestMethod]
        public void Can_Paginate()
        {
            ProductController controller = new ProductController(simulateRepository.Object);
            controller.PageSize = 3;

            //act

            ProductsListViewModel resultat = (ProductsListViewModel)controller.List(null, 2).Model;

            //assert
            Product[] prodArr = resultat.Products.ToArray();
            Console.WriteLine(prodArr.Length);
            Assert.IsTrue(prodArr.Length == 2);
            Assert.AreEqual(prodArr[0].Name, "P4");
            Assert.AreEqual(prodArr[1].Name, "P5");



        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myhelper = new HtmlHelper(new Mock<ViewContext>().Object, new Mock<IViewDataContainer>().Object);

            PagingInfo info = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            MvcHtmlString result = myhelper.PageLinks(info, i => "Page" + i);

            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>" + @"<a class=""selected"" href=""Page2"">2</a>" + @"<a href=""Page3"">3</a>");
        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //
            ProductController pc = new ProductController(simulateRepository.Object);
            pc.PageSize = 3;
            ProductsListViewModel res = (ProductsListViewModel)pc.List(null, 2).Model;

            PagingInfo info = res.PagingInfo;
            Assert.AreEqual(info.CurrentPage, 2);
            Assert.AreEqual(info.ItemsPerPage, 3);
            Assert.AreEqual(info.TotalItems, 5);
            Assert.AreEqual(info.TotalPages, 2);



        }
        [TestMethod]
        public void Can_Filter_Products()
        {
            ProductController controller = new ProductController(simulateRepository.Object);
            controller.PageSize = 3;

            Product[] result = ((controller.List("Cat2", 1).Model) as ProductsListViewModel).Products.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
        [TestMethod]
        public void Can_Create_Categories()
        {
            NavController navControl = new NavController(simulateRepository.Object);

            string[] res = (navControl.Menu().Model as IEnumerable<string>).ToArray();

            Assert.AreEqual(res.Length, 3);
            Assert.AreEqual(res[0], "Cat1");
            Assert.AreEqual(res[1], "Cat2");
            Assert.AreEqual(res[2], "Cat3");


        }
        [TestMethod]
        public void Indicatets_Selected_Category()
        {
            //arrange
            NavController target = new NavController(simulateRepository.Object);
            //act
            target.Menu("Cat2");

            //asserts
            Assert.AreEqual(target.ViewBag.SelectedCategory, "Cat2");

        }
        [TestMethod]
        public void Genarate_Specify_Category()
        {
            ProductController target = new ProductController(simulateRepository.Object);
            target.PageSize = 3;

            //act
            decimal res1 = (target.List("Cat1").Model as ProductsListViewModel).PagingInfo.TotalItems;
            decimal res2 = (target.List("Cat2").Model as ProductsListViewModel).PagingInfo.TotalItems;
            decimal res3 = (target.List("Cat3").Model as ProductsListViewModel).PagingInfo.TotalItems;
            decimal resAll = (target.List(null).Model as ProductsListViewModel).PagingInfo.TotalItems;

            //asserts
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);

        }
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Cart cart = new Cart();
            //act
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 1);

            CartLine[] res = cart.Lines.ToArray();
            //assert
            Assert.AreEqual(res.Length, 2);
            Assert.AreEqual(res[0].Product, prod1);
            Assert.AreEqual(res[1].Product, prod2);
        }
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Line()
        {
            Cart cart = new Cart();
            //act
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 1);
            cart.AddItem(prod1, 10);

            CartLine[] res = cart.Lines.ToArray();
            //assert
            Assert.AreEqual(res.Length, 2);
            Assert.AreEqual(res[0].Quantity, 11);
            Assert.AreEqual(res[1].Quantity, 1);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            Cart cart = new Cart();
            //act
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 1);
            cart.AddItem(prod2, 100);
            cart.AddItem(prod1, 10);

            cart.RemoveLine(prod2);

            //assert
            Assert.AreEqual(cart.Lines.Where(e => e.Product.Name == "P2").Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 1);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            Cart cart = new Cart();
            //act
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 1);
            cart.AddItem(prod2, 100);
            cart.AddItem(prod1, 10);

            cart.RemoveLine(prod2);

            //assert
            Assert.AreEqual(cart.ComputeTotalValue(), 220m);
        }
        [TestMethod]
        public void Can_Clear_Cart()
        {
            Cart cart = new Cart();
            //act
            cart.AddItem(prod1, 1);
            cart.AddItem(prod2, 1);
            cart.AddItem(prod2, 100);
            cart.AddItem(prod1, 10);

            cart.Clear();
            //assert
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Cart cart = new Cart();
            CartController target = new CartController(simulateRepository.Object, null);
            //act
            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductId, 1);

        }
        [TestMethod]
        public void Adding_Goes_To_Cart_Screen()
        {
            Cart cart = new Cart();
            CartController target = new CartController(simulateRepository.Object, null);
            //act
            RedirectToRouteResult res = target.AddToCart(cart, 1, "MyUrl");
            
            Assert.AreEqual(res.RouteValues["action"], "Index");
            Assert.AreEqual(res.RouteValues["returnUrl"], "MyUrl");
        }
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            Cart cart = new Cart();
            CartController target = new CartController(null, null);
            //act
            CartIndexViewModel res = (CartIndexViewModel)target.Index(cart, "MyUrl").ViewData.Model;

            Assert.AreSame(res.Cart, cart);
            Assert.AreEqual(res.ReturnUrl, "MyUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m =>
            m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");
            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m =>
            m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        
    }
}
