using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Reference.Commerce.Site.Features.Market.Services;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using EPiServer.Reference.Commerce.Site.Features.Product.ViewModels;
using EPiServer.Reference.Commerce.Site.Features.Shared.Extensions;
using EPiServer.Reference.Commerce.Site.Features.Shared.Services;
using EPiServer.Reference.Commerce.Site.Infrastructure.Facades;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Commerce.Order;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.Features.Product.Controllers
{
    public class ProductController : ContentController<FashionProduct>
    {
        private readonly IPromotionService _promotionService;
        private readonly IContentLoader _contentLoader;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyservice;
        private readonly IRelationRepository _relationRepository;
        private readonly AppContextFacade _appContext;
        private readonly UrlResolver _urlResolver;
        private readonly FilterPublished _filterPublished;
        private readonly LanguageResolver _languageResolver;
        private readonly bool _isInEditMode;

        private readonly IPriceService _priceService;
        private readonly IPlacedPriceProcessor _placedPriceProcessor;

        public ProductController(
            IPromotionService promotionService,
            IContentLoader contentLoader,
            IPriceService priceService,
            IPlacedPriceProcessor priceProcessor,
            ICurrentMarket currentMarket,
            CurrencyService currencyservice,
            IRelationRepository relationRepository,
            AppContextFacade appContext,
            UrlResolver urlResolver,
            FilterPublished filterPublished,
            LanguageResolver languageResolver,
            IsInEditModeAccessor isInEditModeAccessor)
        {
            _promotionService = promotionService;
            _contentLoader = contentLoader;
            _priceService = priceService;
            _currentMarket = currentMarket;
            _currencyservice = currencyservice;
            _relationRepository = relationRepository;
            _appContext = appContext;
            _urlResolver = urlResolver;
            _languageResolver = languageResolver;
            _isInEditMode = isInEditModeAccessor();
            _filterPublished = filterPublished;

            _placedPriceProcessor = priceProcessor;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(FashionProduct currentContent, string variationCode = "", bool quickview = false)
        {
            var variations = GetVariations(currentContent).ToList();
            if (_isInEditMode && !variations.Any())
            {
                var productWithoutVariation = new FashionProductViewModel
                {
                    Product = currentContent,
                    Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver)
                };
                return Request.IsAjaxRequest() ? PartialView("ProductWithoutVariation", productWithoutVariation) : (ActionResult)View("ProductWithoutVariation", productWithoutVariation);
            }

            FashionVariant variation;
            if (!TryGetFashionVariant(variations, variationCode, out variation))
            {
                return HttpNotFound();
            }

            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();

            var defaultPrice = GetCustomerDefaultPrice(variation, market, currency); //GetDefaultPrice(variation, market, currency);
            var discountedPrice = GetDiscountPrice(defaultPrice, market, currency);

            var otherPrices = GetCustomerDefaultPrice(variation, market, currency);

            var viewModel = new FashionProductViewModel
            {
                Product = currentContent,
                Variation = variation,
                ListingPrice = defaultPrice != null ? defaultPrice.UnitPrice : new Money(0, currency),
                DiscountedPrice = discountedPrice,
                Colors = variations
                    .Where(x => x.Size != null)
                    .GroupBy(x => x.Color)
                    .Select(g => new SelectListItem                    
                    {
                        Selected = false,
                        Text = g.First().Color,
                        Value = g.First().Color
                    })
                    .ToList(),
                Sizes = variations
                    .Where(x => x.Color != null && x.Color.Equals(variation.Color, StringComparison.OrdinalIgnoreCase))
                    .Select(x => new SelectListItem
                    {
                        Selected = false,
                        Text = x.Size,
                        Value = x.Size
                    })
                    .ToList(),
                Color = variation.Color,
                Size = variation.Size,
                Images = variation.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                IsAvailable = defaultPrice != null
            };

            if (quickview)
            {
                return PartialView("Quickview", viewModel);
            }

            return Request.IsAjaxRequest() ? PartialView(viewModel) : (ActionResult)View(viewModel);
        }

        [HttpPost]
        public ActionResult SelectVariant(FashionProduct currentContent, string color, string size, bool quickview = false)
        {
            var variations = GetVariations(currentContent);

            FashionVariant variation;
            if (TryGetFashionVariantByColorAndSize(variations, color, size, out variation)
                || TryGetFashionVariantByColorAndSize(variations, color, string.Empty, out variation))//if we cannot find variation with exactly both color and size then we will try to get variation by color only
            {
                return RedirectToAction("Index", new { variationCode = variation.Code, quickview });
            }

            return HttpNotFound();
        }

        private IEnumerable<FashionVariant> GetVariations(FashionProduct currentContent)
        {
            return _contentLoader
                .GetItems(currentContent.GetVariants(_relationRepository), _languageResolver.GetPreferredCulture())
                .Cast<FashionVariant>()
                .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !_filterPublished.ShouldFilter(v));
        }

        private static bool TryGetFashionVariant(IEnumerable<FashionVariant> variations, string variationCode, out FashionVariant variation)
        {
            variation = !string.IsNullOrEmpty(variationCode) ?
                variations.FirstOrDefault(x => x.Code == variationCode) :
                variations.FirstOrDefault();

            return variation != null;
        }

        private static bool TryGetFashionVariantByColorAndSize(IEnumerable<FashionVariant> variations, string color, string size, out FashionVariant variation)
        {
            variation = variations.FirstOrDefault(x =>
                (string.IsNullOrEmpty(color) || x.Color.Equals(color, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(size) || x.Size.Equals(size, StringComparison.OrdinalIgnoreCase)));

            return variation != null;
        }

        private IPriceValue GetDefaultPrice(FashionVariant variation, IMarket market, Currency currency)
        {
            return _priceService.GetDefaultPrice(
                market.MarketId,
                DateTime.Now,
                new CatalogKey(_appContext.ApplicationId, variation.Code),
                currency);

        }

        private IPriceValue GetCustomerDefaultPrice(FashionVariant variation, IMarket market, Currency currency)
        {
            var customerContact = CustomerContext.Current.CurrentContact;

            if (customerContact == null)
                return GetDefaultPrice(variation, market, currency);

            var userGroup = customerContact.CustomerGroup;
            //var priceGroup2 = "Distributor";

            var customerPricing = new List<CustomerPricing>()
            {
                CustomerPricing.AllCustomers
            };

            //If user is logged in
            if (customerContact != null)
            {
                //object userKey = this._mapUserKey.ToUserKey(customerContact.UserId);
                object userKey = customerContact.UserId;
                if (userKey != null && !string.IsNullOrWhiteSpace(userKey.ToString()))
                    customerPricing.Add(new CustomerPricing(CustomerPricing.PriceType.UserName, userKey.ToString()));
                if (!string.IsNullOrEmpty(customerContact.CustomerGroup))
                    customerPricing.Add(new CustomerPricing(CustomerPricing.PriceType.PriceGroup, customerContact.CustomerGroup));
            }

            var priceFilter = new PriceFilter()
            {
                Quantity = 0,
                Currencies = new List<Currency> {currency},
                CustomerPricing = customerPricing
            };

            var prices = _priceService.GetPrices(market.MarketId, DateTime.Now,
                new CatalogKey(_appContext.ApplicationId, variation.Code), priceFilter);

            //returns the lowest available price based
            if (prices.Any())
            {
                return prices.OrderBy(x => x.UnitPrice.Amount).First();
            }

            return null;
        }

        private Money? GetDiscountPrice(IPriceValue defaultPrice, IMarket market, Currency currency)
        {
            if (defaultPrice == null)
            {
                return null;
            }

            var discountPrice = _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId, currency);

            if (discountPrice != null)
                return discountPrice.UnitPrice;
            else
                return null;
        }
    }
}