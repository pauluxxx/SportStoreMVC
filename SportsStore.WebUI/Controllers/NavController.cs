﻿using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        //

        private IProductRepository repository;

        public NavController(IProductRepository rep)
        {
            repository = rep;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.
                Products
                .Select(x => x.Category)
                .Select(x=>x.Trim())
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }

    }
}
