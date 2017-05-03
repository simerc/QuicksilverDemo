using System.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Profile.Pages;
using EPiServer.Web.Mvc;
using EPiServer.Reference.Commerce.Site.Features.Profile.ViewModels;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.Features.Profile.Controllers
{
    [Authorize]
    public class ProfilePageController : PageController<ProfilePage>
    {
        private CustomerContextFacade _customerContext;

        public ProfilePageController(CustomerContextFacade facade)
        {
            _customerContext = facade;
        }

        public ActionResult Index(ProfilePage currentPage)
        {
            var cust = CustomerContext.Current.CurrentContact;
           
            var viewModel = new ProfilePageViewModel { CurrentPage = currentPage };

            viewModel.CustomerGroup = cust.EffectiveCustomerGroup;

            return View(viewModel);
        }
    }
}