using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Reference.Commerce.Site.Features.Login.Services;
using EPiServer.Reference.Commerce.Site.Features.Registration.Blocks;
using EPiServer.Reference.Commerce.Site.Features.Registration.Models;
using EPiServer.Reference.Commerce.Site.Features.Shared.Controllers;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Registration.Pages;
using EPiServer.Reference.Commerce.Site.Features.Search.Pages;
using EPiServer.Reference.Commerce.Site.Features.Shared.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Standard.Pages;
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Standard.Controllers
{
    public class StandardPageController : PageController<StandardPage>
    {
        private readonly IContentLoader _loader;

        public StandardPageController(IContentLoader loader)
        {
            _loader = loader;
        }

        public ActionResult Index(StandardPage currentPage)
        {
            var model = new PageViewModel<StandardPage>();
            model.CurrentPage = currentPage;

            return View(model);
        }

    }
}