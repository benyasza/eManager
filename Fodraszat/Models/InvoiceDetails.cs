using Fodraszat.Models.InvoiceEntities;
using System.Collections.Generic;

namespace Fodraszat.Models
{
    public class InvoiceDetails
    {
        public int InvoiceNumber { get; set; }

        public int TotalCost { get; set; }

        public IEnumerable<JobEntity> Jobs { get; set; }

        public IEnumerable<MaterialEntity> Materials { get; set; }

        public IEnumerable<PurchasedProductEntity> Products { get; set; }
    }
}