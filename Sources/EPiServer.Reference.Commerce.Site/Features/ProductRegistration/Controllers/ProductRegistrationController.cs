using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.ProductRegistration.Pages;
using EPiServer.Reference.Commerce.Site.Features.ProductRegistration.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Profile.Pages;
using EPiServer.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Profile.ViewModels;

namespace EPiServer.Reference.Commerce.Site.Features.ProductRegistration.Controllers
{
    [Authorize]
    public class ProductRegistration : PageController<ProductRegistrationPage>
    {
        public ActionResult Index(ProductRegistrationPage currentPage)
        {
            var viewModel = new ProductRegistrationViewModel { CurrentPage = currentPage };
            return View(viewModel);
        }
    }
}