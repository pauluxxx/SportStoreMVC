using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private IProductRepository rep;
        public AdminController(IProductRepository rep) {
            this.rep = rep;
        }
        public ViewResult Index()
        {
            return View(rep.Products);
        }
        public ViewResult Edit(int ProductId)
        {
            Product prod = rep.Products.FirstOrDefault(m=>m.ProductId==ProductId);
            return View(prod);
        }
        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            if(ModelState.IsValid){
                if (image !=null)
                {
                    product.ImageData = new byte[image.ContentLength];
                    product.ImageMimeType = image.ContentType;
                    image.InputStream.Read(product.ImageData,0,image.ContentLength);
                }
                rep.SaveProduct(product);
                TempData["message"] = string.Format("Product : {0} is changed",product.Name);
                return RedirectToAction("Index");
            }
            else
                return View(product);
        }
        public ViewResult Create()
        {
            return View("Edit",new Product());
        }
        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProd = rep.DeleteProduct(productId);
            if (deletedProd!=null)
            {
                TempData["message"] = string.Format("The product {0} is deleted!",deletedProd.Name);
            }
           return RedirectToAction("Index");
        }
    }
}
