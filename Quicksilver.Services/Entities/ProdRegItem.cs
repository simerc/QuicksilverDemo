using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Quicksilver.Services.Entities
{
    [DataContract]
    public class ProdRegItem
    {
        [DataMember]
        public int ProductRegistrationId { get; set; }

        //which will be a guid
        //
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string ProductName { get; set; }


        // Date in format dd-MM-yyyy
        //
        [DataMember]
        public string RegisteredDate { get; set; }

        // 1 = Online
        // 2 = Instore
        [DataMember]
        public string PurchaseTypeCode { get; set; }
    }
}