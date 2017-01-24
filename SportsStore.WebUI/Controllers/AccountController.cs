using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        IAuthProvider authProvider;
        public AccountController(IAuthProvider providerParam) {
            authProvider = providerParam;
        }
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel logMod, string returnUrl){
            if (ModelState.IsValid)
            {
                if (authProvider.Authentificete(logMod.UserName,logMod.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index","Admin"));
                }
                else
                {
                    ModelState.AddModelError("","Incorect pasword or login");
                    return View();
                }
                 
            }else
                return View();
        }

    }
}
