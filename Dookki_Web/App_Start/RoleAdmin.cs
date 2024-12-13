using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace Dookki_Web.App_Start
{
    public class RoleAdmin : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpRequestBase = new HttpRequestWrapper(HttpContext.Current.Request);
            var user = CookiesConfig.GetAdmin(httpRequestBase);
            if (user == null)
            {
                //Dieu huong ve trang login
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new
                    {
                        controller = "AdminHome",
                        action = "Login",
                        area = "Admin"
                    }));
                return;
            }
            return;
        }

    }
}