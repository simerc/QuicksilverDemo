using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    public class CustomViewEngine : RazorViewEngine
    {
        private readonly IContentLoader _loader;

        public CustomViewEngine() : base()
        {
            _loader = ServiceLocator.Current.GetInstance<IContentLoader>();

            //ViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Themes/" + baseThemeName + "/Views/Shared/{0}.cshtml" };
            //PartialViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Themes/" + baseThemeName + "/Views/Shared/{0}.cshtml" };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
                throw new ArgumentNullException(nameof(controllerContext));
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName");

            var settingsPage = _loader.Get<StartPage>(SiteDefinition.Current.StartPage);
            var themeName = settingsPage.SiteTheme;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            if (!string.IsNullOrEmpty(themeName) && !viewName.Contains("/"))
            {
                var viewPath = String.Format("~/Views/{0}/{1}/{2}.cshtml", themeName, controllerName, viewName);
                //If the view file doesn't exists in the folder look at the shared folder
                var absolutePath = HttpContext.Current.Server.MapPath(viewPath);
                if (!System.IO.File.Exists(absolutePath))
                {
                    viewPath = String.Format("~/Views/{0}/shared/{1}.cshtml", themeName, viewName);
                    absolutePath = HttpContext.Current.Server.MapPath(viewPath);
                    if (!System.IO.File.Exists(absolutePath))   
                    {
                        throw new Exception(string.Format("View {0} doesn't exists.", viewName));
                    }
                }
                return new ViewEngineResult(this.CreateView(controllerContext, viewPath, string.Empty), (IViewEngine)this);
            }
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        //public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        //{
        //    if (controllerContext == null)
        //        throw new ArgumentNullException(nameof(controllerContext));
        //    if (string.IsNullOrEmpty(partialViewName))
        //        throw new ArgumentException("partialViewName");
        //    var currentSite = Sitecore.Context.Site;
        //    var themeName = currentSite.Properties["themeName"];
        //    string controllerName = controllerContext.RouteData.GetRequiredString("controller");
        //    if (!string.IsNullOrEmpty(themeName) && !partialViewName.Contains("/"))
        //    {
        //        var partilaViewPath = $"~/themes/{themeName}/views/{controllerName}/{partialViewName}.cshtml";
        //        //If the view file doesn't exists in the folder look at the shared folder
        //        var absolutePath = HttpContext.Current.Server.MapPath(partilaViewPath);
        //        if (!System.IO.File.Exists(absolutePath))
        //        {
        //            partilaViewPath = $"~/themes/{themeName}/views/shared/{partialViewName}.cshtml";
        //            absolutePath = HttpContext.Current.Server.MapPath(partilaViewPath);
        //            if (!System.IO.File.Exists(absolutePath))
        //            {
        //                throw new Exception(string.Format("View {0} doesn't exists.", partialViewName));
        //            }
        //        }
        //        return new ViewEngineResult(this.CreatePartialView(controllerContext, partilaViewPath), (IViewEngine)this);
        //    }
        //    return base.FindPartialView(controllerContext, partialViewName, useCache);
        //}

    }
}