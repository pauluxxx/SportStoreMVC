using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repositoryProducts;
        public int PageSize = 4;
        public ProductController(IProductRepository repParam)
        {
            repositoryProducts = repParam;
        }

        public ViewResult List(string category, int page = 1)
        {

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repositoryProducts.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    repositoryProducts.Products.Count() :
                    repositoryProducts.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }
        public FileContentResult GetImage(int productId)
        {
            Product prod = repositoryProducts.Products.FirstOrDefault(e=>e.ProductId==productId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
                return null;
        }
    }
}
