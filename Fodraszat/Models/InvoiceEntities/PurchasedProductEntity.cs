using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fodraszat.Models.InvoiceEntities
{
    public class PurchasedProductEntity
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}