using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Dto;
using Mediachase.Commerce.Catalog.Managers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using EPiServer.Commerce.Order.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;

namespace Mediachase.Commerce.Workflow.Activities
{
    internal static class OrderFormHelper
    {
        /// <summary>
        /// Gets the taxes for order form
        /// </summary>
        /// <param name="form">The order form.</param>
        /// <param name="marketId">The order market Id.</param>
        /// <param name="currency">The order currency.</param>
        /// <returns></returns>
        public static void CalculateTaxes(OrderForm form, MarketId marketId, Currency currency)
        {
            decimal totalTaxes = 0;
            foreach (Shipment shipment in form.Shipments)
            {
                var shippingTax = 0m;
                decimal shippingCost = shipment.ShippingSubTotal - shipment.ShippingDiscountAmount;

                var lineItems = Shipment.GetShipmentLineItems(shipment);
                // Calculate sales and shipping taxes per items
                foreach (LineItem item in lineItems)
                {
                    // Try getting an address
                    OrderAddress address = GetAddressByName(form, shipment.ShippingAddressId);
                    if (address != null) // no taxes if there is no address
                    {
                        // Try getting an entry
                        CatalogEntryDto entryDto = CatalogContext.Current.GetCatalogEntryDto(item.Code, new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.Variations));
                        if (entryDto.CatalogEntry.Count > 0) // no entry, no tax category, no tax
                        {
                            CatalogEntryDto.VariationRow[] variationRows = entryDto.CatalogEntry[0].GetVariationRows();
                            if (variationRows.Length > 0)
                            {
                                string taxCategory = CatalogTaxManager.GetTaxCategoryNameById(variationRows[0].TaxCategoryId);
                                IMarket market = ServiceLocator.Current.GetInstance<IMarketService>().GetMarket(marketId);
                                TaxValue[] taxes = OrderContext.Current.GetTaxes(Guid.Empty, taxCategory, market.DefaultLanguage.Name, address.CountryCode, address.State, address.PostalCode, address.RegionCode, String.Empty, address.City);

                                if (taxes.Length > 0)
                                {
                                    // calculate quantity of item in current shipment
                                    var quantity = Shipment.GetLineItemQuantity(shipment, item.LineItemId);
                                    var totalShipmentLineItemsQuantity = lineItems.Sum(l => Shipment.GetLineItemQuantity(shipment, item.LineItemId));

                                    // price exclude tax for 1 line item
                                    var lineItemPricesExcTax = GetPriceExcludingTax(item, currency);

                                    if (OrderForm.IsReturnOrderForm(form))
                                    {
                                        totalTaxes += GetTaxesAmount(taxes, TaxType.SalesTax, lineItemPricesExcTax, currency) * item.ReturnQuantity;
                                    }
                                    else
                                    {
                                        totalTaxes += GetTaxesAmount(taxes, TaxType.SalesTax, lineItemPricesExcTax, currency) * quantity;
                                    }

                                    var shipmentItemsPricesExcTax = lineItemPricesExcTax * quantity;
                                    var itemShippingCost = shipment.SubTotal == 0 ? currency.Round(quantity / totalShipmentLineItemsQuantity * shippingCost)  : currency.Round(shipmentItemsPricesExcTax / shipment.SubTotal * shippingCost);
                                    shippingTax += GetTaxesAmount(taxes, TaxType.ShippingTax, itemShippingCost, currency);
                                }
                            }
                        }
                    }
                }

                shipment.ShippingTax = shippingTax;
                totalTaxes += shipment.ShippingTax;
            }

            form.TaxTotal = totalTaxes;
        }

        /// <summary>
        /// Get Item Price Excluding Tax
        /// </summary>
        /// <param name="item">The line item</param>
        /// <param name="currency">The currency</param>
        /// <returns>Item price excluding tax</returns>
        private static decimal GetPriceExcludingTax(ILineItem item, Currency currency)
        {
            return item.PlacedPrice - currency.Round(item.TryGetDiscountValue(x => x.OrderAmount) / item.Quantity) - currency.Round(item.TryGetDiscountValue(x => x.EntryAmount) / item.Quantity);            
        }

        /// <summary>
        /// Calculate the tax for specific tax type
        /// </summary>
        /// <param name="taxes">The taxes</param>
        /// <param name="taxType">The tax type</param>
        /// <param name="unitPrice">The item price excluding taxes or the shipping cost</param>        
        /// <param name="currency">The tax currency</param>
        /// <returns>The tax value</returns>
        private static decimal GetTaxesAmount(IEnumerable<ITaxValue> taxes, TaxType taxType, decimal unitPrice, Currency currency)
        {
            return taxes.Where(x => x.TaxType == taxType).Sum(x => currency.Percentage(unitPrice, x.Percentage));
        }

        /// <summary>
        /// Gets the name of the address by name.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static OrderAddress GetAddressByName(OrderForm form, string name)
        {
            foreach (OrderAddress address in form.Parent.OrderAddresses)
            {
                if (address.Name.Equals(name))
                    return address;
            }

            return null;
        }
    }
}
