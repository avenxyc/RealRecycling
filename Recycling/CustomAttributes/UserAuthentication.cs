using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;

namespace Recycling.CustomAttributes
{
    public class UserAuthentication : FilterAttribute, IAuthenticationFilter
    {
        public string Role1 { get; set; }
        public string Role2 { get; set; }

        public void OnAuthentication(AuthenticationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                HttpCookie authCookie = context.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                var role = ticket.UserData;

                if (Role1 == role || Role2 == role)
                {
                    // Do something
                }
                else
                {
                    context.Result = new HttpUnauthorizedResult(); // mark unauthorized
                }
            }
            else
            {

                context.Result = new HttpUnauthorizedResult(); // mark unauthorized
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            if (context.Result == null || context.Result is HttpUnauthorizedResult)
            {
                context.Result = new RedirectToRouteResult("Default",
                    new System.Web.Routing.RouteValueDictionary{
                        {"controller", "User"},
                        {"action", "Warning"},
                        {"returnUrl", context.HttpContext.Request.RawUrl}
                    });
            }
        }
    }
}