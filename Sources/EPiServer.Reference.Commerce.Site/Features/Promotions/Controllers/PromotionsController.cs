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
using EPiServer.Web.Mvc;

namespace EPiServer.Reference.Commerce.Site.Features.Promotions.Controllers
{
    public class PromotionsController : PageController<PromotionsPage>
    {
        private readonly IContentLoader _loader;

        public PromotionsController(IContentLoader loader)
        {
            _loader = loader;
        }

        public ActionResult Index()
        {
            var result = "";

            return Content(result);
        }

    }
}