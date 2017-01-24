using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStore.WebUI.Models
{
    public class CartIndexViewModel
    {
        public string ReturnUrl { get; set; }

        public Cart Cart { get; set; }
    }
}
