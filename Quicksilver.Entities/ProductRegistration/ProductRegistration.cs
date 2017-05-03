using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Quicksilver.Entities.ProductRegistration
{
    public class ProductRegistration
    {
        public int ProductRegistrationId { get; set; }

        public Guid UserId { get; set; }

        public string ProductName { get; set; }

        public DateTime RegisteredDate { get; set; }

        public PurchaseTypeCode PurchaseType { get; set; }
    }
}
