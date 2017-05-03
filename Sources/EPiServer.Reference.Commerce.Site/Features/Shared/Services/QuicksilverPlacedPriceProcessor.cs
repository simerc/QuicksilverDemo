

// Decompiled with JetBrains decompiler
// Type: EPiServer.Commerce.Order.DefaultPlacedPriceProcessor
// Assembly: EPiServer.Business.Commerce, Version=10.3.0.0, Culture=neutral, PublicKeyToken=8fe83dea738b45b7
// MVID: 67650A90-A769-4182-A82A-449C47304533
// Assembly location: C:\projects\Quicksilver-master\Sources\EPiServer.Reference.Commerce.Site\bin\EPiServer.Business.Commerce.dll

using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;

namespace EPiServer.Reference.Commerce.Site.Features.Shared.Services
{
    /// <summary>
    /// 
    /// This class is injected into the ValidateCart method on the CartService, this allows us to control how the prices are manipulated
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// Service for updating and retrieving <see cref="P:EPiServer.Commerce.Order.ILineItem.PlacedPrice" /> for <see cref="T:EPiServer.Commerce.Order.IOrderGroup" />.
    /// </summary>
    [ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton, ServiceType = typeof(IPlacedPriceProcessor))]
    public class QuicksilverPlacedPriceProcessor : IPlacedPriceProcessor
    {
        private readonly IPriceService _priceService;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly MapUserKey _mapUserKey;

        public QuicksilverPlacedPriceProcessor(IPriceService priceService, IContentLoader contentLoader, ReferenceConverter referenceConverter, MapUserKey mapUserKey)
        {
            this._priceService = priceService;
            this._contentLoader = contentLoader;
            this._referenceConverter = referenceConverter;
            this._mapUserKey = mapUserKey;
        }

        /// <summary>
        /// Updates the <see cref="T:EPiServer.Commerce.Order.ILineItem" /> item placed price or raises <see cref="T:EPiServer.Commerce.Order.ValidationIssue" /> if their is no valid price.
        /// </summary>
        /// <param name="lineItem">The line item.</param>
        /// <param name="customerContact"></param>
        /// <param name="market">The market.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="onValidationError">A callback that is invoked if a validation issue is detected.</param>
        /// <returns>False if is there is no valid price</returns>
        public virtual bool UpdatePlacedPrice(ILineItem lineItem, CustomerContact customerContact, IMarket market, Mediachase.Commerce.Currency currency, Action<ILineItem, ValidationIssue> onValidationError)
        {
            EntryContentBase entryContent = lineItem.GetEntryContent(this._referenceConverter, this._contentLoader);
            if (entryContent == null)
            {
                onValidationError(lineItem, ValidationIssue.RemovedDueToUnavailableItem);
                return false;
            }
            Money? placedPrice = this.GetPlacedPrice(entryContent, lineItem.Quantity, customerContact, market, currency);
            if (placedPrice.HasValue)
            {
                if (new Money(currency.Round(lineItem.PlacedPrice), currency) == placedPrice.Value)
                    return true;
                onValidationError(lineItem, ValidationIssue.PlacedPricedChanged);
                lineItem.PlacedPrice = placedPrice.Value.Amount;
                return true;
            }
            onValidationError(lineItem, ValidationIssue.RemovedDueToInvalidPrice);
            return false;
        }

        /// <summary>Gets the placed price.</summary>
        /// <param name="entry">The entry.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="customerContact">The customer contact.</param>
        /// <param name="market">The market.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>A <see cref="T:Mediachase.Commerce.Money" /></returns>
        public virtual Money? GetPlacedPrice(EntryContentBase entry, Decimal quantity, CustomerContact customerContact, IMarket market, Mediachase.Commerce.Currency currency)
        {
            List<CustomerPricing> customerPricingList1 = new List<CustomerPricing>()
            {
                CustomerPricing.AllCustomers
            };

            //If user is logged in
            if (customerContact != null)
            {
                object userKey = this._mapUserKey.ToUserKey(customerContact.UserId);
                if (userKey != null && !string.IsNullOrWhiteSpace(userKey.ToString()))
                    customerPricingList1.Add(new CustomerPricing(CustomerPricing.PriceType.UserName, userKey.ToString()));
                if (!string.IsNullOrEmpty(customerContact.CustomerGroup))
                    customerPricingList1.Add(new CustomerPricing(CustomerPricing.PriceType.PriceGroup, customerContact.CustomerGroup));
            }

            PriceFilter priceFilter = new PriceFilter();
            priceFilter.Currencies = new List<Currency>
                {
                    currency
                };

            Decimal? nullable = new Decimal?(quantity);
            priceFilter.Quantity = nullable;
            List<CustomerPricing> customerPricingList2 = customerPricingList1;
            priceFilter.CustomerPricing = (IEnumerable<CustomerPricing>)customerPricingList2;
            int num = 0;
            priceFilter.ReturnCustomerPricing = num != 0;
            PriceFilter filter = priceFilter;
            IPriceValue priceValue = this._priceService.GetPrices(market.MarketId, DateTime.UtcNow, new CatalogKey(new Guid(entry.ApplicationId), entry.Code), filter).OrderBy<IPriceValue, Money>((Func<IPriceValue, Money>)(pv => pv.UnitPrice)).FirstOrDefault<IPriceValue>();
            if (priceValue != null)
                return new Money?(priceValue.UnitPrice);
            return new Money?();
        }
    }
}
