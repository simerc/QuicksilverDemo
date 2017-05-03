using System.Collections.Generic;
using Quicksilver.Entities.ProductRegistration;
using System.ServiceModel;
using Quicksilver.Services.Entities;

namespace Quicksilver.Interfaces.Services
{
    [ServiceContract]
    public interface IProductRegistrationService
    {
        [OperationContract]
        List<ProdRegItem> GetAllProductRegistrations();

        [OperationContract]
        ProdRegItem GetProductRegistration(int id);
    }
}
