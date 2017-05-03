using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Infrastructure.Owin;
using EPiServer.Security;
using Microsoft.Owin.Security;

namespace EPiServer.Reference.Commerce.Site.Features.Login.Controllers
{
    public class AccountController : Controller
    {
        //// GET: Account
        //public void SignIn()
        //{
        //    if (!Request.IsAuthenticated)
        //    {
        //        HttpContext.GetOwinContext().Authentication.Challenge(
        //                    new AuthenticationProperties() { RedirectUri = "/" }, Startup.SignInPolicyId);
        //    }
        //}

        //public void SignUp()
        //{
        //    if (!Request.IsAuthenticated)
        //    {
        //        HttpContext.GetOwinContext().Authentication.Challenge(
        //            new AuthenticationProperties() { RedirectUri = "/" }, Startup.SignUpPolicyId);
        //    }
        //}

        //public void UserProfile()
        //{
        //    if (Request.IsAuthenticated)
        //    {
        //        HttpContext.GetOwinContext().Authentication.Challenge(
        //            new AuthenticationProperties() { RedirectUri = "/" }, Startup.ProfilePolicyId);
        //    }
        //}
    }
}