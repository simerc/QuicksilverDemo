
using System.Collections.Generic;
using Quicksilver.Business.ProductRegistrationService;
using Quicksilver.Entities.ProductRegistration;
using IProductRegistrationService = Quicksilver.Interfaces.Services.IProductRegistrationService;

namespace Quicksilver.Business.Services
{
    public class ProductRegistrationService : IProductRegistrationService
    {
        public ProductRegistration GetProductRegistration(int id)
        {
            var service = new ProductRegistrationService();

            return null;

            //service.GetAllProductRegistrations
        }

        public List<ProductRegistration> GetAllProductRegistrations()
        {
            return null;
        }
    }
}
