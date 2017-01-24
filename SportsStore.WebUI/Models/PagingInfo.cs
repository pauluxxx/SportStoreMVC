using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models
{
    public class PagingInfo
    {

        public int TotalPages {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        public decimal TotalItems { get; set; }
    }
}