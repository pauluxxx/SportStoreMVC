using System;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {         // Arrange - create a Product with image data   
            Product prod = new Product { ProductId = 2, Name = "Test", ImageData = new byte[] { }, ImageMimeType = "image/png" };
            Mock<IProductRepository> mock = new Mock<IProductRepository>(); mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductId = 1, Name = "P1" }, prod, new Product { ProductId = 3, Name = "P3" } }.AsQueryable());
            ProductController target = new ProductController(mock.Object);
            ActionResult result = target.GetImage(2);
            // Assert       
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>(); mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductId = 1, Name = "P1" }, new Product { ProductId = 2, Name = "P2" } }.AsQueryable());
            ProductController target = new ProductController(mock.Object);
            ActionResult result = target.GetImage(100);
            Assert.IsNull(result);
        }
    }
}
