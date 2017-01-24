using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.HTMLHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this 
            HtmlHelper html, PagingInfo info,
            Func<int ,string>pageUrl
            ) {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i <= info.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href",pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    if (i==info.CurrentPage)
                        tag.AddCssClass("selected");
                    sb.Append(tag.ToString());
                    
                }

                return MvcHtmlString.Create(sb.ToString());
            }
    }
}