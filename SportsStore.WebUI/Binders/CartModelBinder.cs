using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Binders
{
    public class CartModelBinder:IModelBinder
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext cContext,
            ModelBindingContext mbContext) { 
            //get form session
                Cart cart = (Cart)cContext.HttpContext.Session[sessionKey];
                if (cart==null)
                {
                    cart = new Cart();
                    cContext.HttpContext.Session[sessionKey] = cart;
                }
                return cart;

        }
    }
}