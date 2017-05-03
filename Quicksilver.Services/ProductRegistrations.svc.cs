using System;
using System.Collections.Generic;
using Quicksilver.Entities.ProductRegistration;
using Quicksilver.Interfaces.Services;
using Quicksilver.Services.Entities;

namespace Quicksilver.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductRegistrations" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductRegistrations.svc or ProductRegistrations.svc.cs at the Solution Explorer and start debugging.
    public class ProductRegistrations : IProductRegistrationService
    {
        public List<ProdRegItem> GetAllProductRegistrations()
        {
            return new List<ProdRegItem>()
            {
                new ProdRegItem
                {
                    ProductRegistrationId = 1,
                    ProductName = "Product 1",
                    RegisteredDate = DateTime.Now.ToString("dd-MM-yyyy"),
                    UserId = new Guid().ToString(),
                    PurchaseTypeCode = "1"
                },

                new ProdRegItem
                {
                    ProductRegistrationId = 2,
                    ProductName = "Product 2",
                    RegisteredDate = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy"),
                    UserId = new Guid().ToString(),
                    PurchaseTypeCode = "1"
                }
            };
        }

        public ProdRegItem GetProductRegistration(int id)
        {
            return new ProdRegItem
            {
                ProductRegistrationId = id,
                ProductName = "Product " + id.ToString(),
                RegisteredDate = DateTime.Now.AddDays(-id).ToString("dd-MM-yyyy")
            };
        }
    }
}
