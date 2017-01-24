using SportsStore.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SportsStore.WebUI.Infrastructure.Concrete
{
    public class FormAuthProvider:IAuthProvider
    {
        public bool Authentificete(string userName, string password)
        {
            bool res = FormsAuthentication.Authenticate(userName,password);
            if (res)
            {
                FormsAuthentication.SetAuthCookie(userName,false);
            }
            return res;
        }
    }
}