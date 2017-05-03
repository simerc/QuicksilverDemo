using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quicksilver.Entities.ProductRegistration;

namespace Quicksilver.Interfaces.Services
{
    public interface IProductRegistrationService
    {
        ProductRegistration GetProductRegistration(int id);
        List<ProductRegistration> GetAllProductRegistrations();
    }
}
