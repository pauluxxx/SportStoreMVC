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
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor processor;
        public CartController(IProductRepository rep,IOrderProcessor proc)
        {
            repository = rep;
            processor = proc;
        }
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product prod = repository.Products
                .FirstOrDefault(e => e.ProductId == productId);
            if (prod != null)
            {
                cart.AddItem(prod, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product prod = repository.Products
                .FirstOrDefault(e => e.ProductId == productId);
            if (prod != null)
            {
                cart.RemoveLine(prod);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails spDetails) {
            if (cart.Lines.Count()==0)
            {
                ModelState.AddModelError("","Sory your cart is empty!");    
            }
            if (ModelState.IsValid)
            {
                processor.ProcessOrder(cart,spDetails);
                cart.Clear();
                return View("Completed!");
            }else
                return View(spDetails);
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        public PartialViewResult Summary(Cart cart) { return PartialView(cart); }
    }
}
