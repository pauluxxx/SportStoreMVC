using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        static Mock<IProductRepository> mock = new Mock<IProductRepository>();
        static AdminController target = new AdminController(mock.Object);
        public AdminTests()
        {
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ ProductId=1,Name="P1"},
                new Product{ ProductId=2,Name="P2"},
                new Product{ ProductId=3,Name="P3"},
            }.AsQueryable());
        }
        [TestMethod]
        public void Index_Contains_All_Prods()
        {
            AdminController admn = new AdminController(mock.Object);
            Product[] res = ((IEnumerable<Product>)admn.Index().ViewData.Model).ToArray();
            Assert.AreEqual(res.Length, 3);
            Assert.AreEqual(res[0].Name, "P1");
            Assert.AreEqual(res[1].Name, "P2");
            Assert.AreEqual(res[2].Name, "P3");

        }
        [TestMethod]
        public void Can_Edit_Product() {
            Product res = (Product)(target.Edit(1).Model);
            Product res2 = (Product)(target.Edit(2).Model);
            Assert.AreEqual(res.Name,"P1");
            Assert.AreEqual(res2.Name,"P2");
        }
        [TestMethod]
        public void Cannot_Edit_UnIdexed() {
            Assert.IsNotNull(target.Edit(6).ViewData.Model);   
        }
        [TestMethod]
        public void Can_Save_Valid_Values() {
            Product product = new Product { Name="P1" };
            ActionResult ress = target.Edit(product,null);
            //check that the repository is changed
            mock.Verify(e=>e.SaveProduct(product));
            Assert.IsNotInstanceOfType(ress,typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Insert_Invalid_Changes() {
            Product prod = new Product {Name="P1" };
            target.ModelState.AddModelError("error","error");
            ActionResult res = target.Edit(prod,null);
            mock.Verify(e=>e.SaveProduct(It.IsAny<Product>()),Times.Once());
            Assert.IsInstanceOfType(res,typeof(ViewResult));
        }
        [TestMethod]
        public void Can_Delete_Valid_Products() {
            target.Delete(1);
           mock.Verify(e=>e.DeleteProduct(1));
        }
        [TestMethod]
        public void Can_Login_With_Valid() {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(e=>e.Authentificete("admin","12345")).Returns(true);
            LoginViewModel log = new LoginViewModel
            {
                UserName = "admin",
                Password = "12345"
            };
            AccountController target = new AccountController(mock.Object);
            ActionResult res = target.Login(log,"/MyUrl");
            Assert.IsInstanceOfType(res,typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)res).Url); 
        }
         [TestMethod]     public void Cannot_Login_With_Invalid_Credentials()     {       // Arrange - create a mock authentication provider   
             Mock<IAuthProvider> mock = new Mock<IAuthProvider>();   
             mock.Setup(m => m.Authentificete("badUser", "badPass")).Returns(false);      
             // Arrange - create the view model    
             LoginViewModel model = new LoginViewModel       {         UserName = "badUser",         Password = "badPass"       };     
             // Arrange - create the controller     
             AccountController target = new AccountController(mock.Object);   
             // Act - authenticate using valid credentials      
             ActionResult result = target.Login(model, "/MyURL");       // Assert      
             Assert.IsInstanceOfType(result, typeof(ViewResult));    
             Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);     }
    }
}
