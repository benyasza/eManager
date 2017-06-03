using Fodraszat.Models.InvoiceEntities;
using System.Collections.Generic;

namespace Fodraszat.Models
{
    public class InvoiceDetails
    {
        public int InvoiceNumber { get; set; }

        public int TotalPrice { get; set; }

        public int PriceWithDiscount { get; set; }

        public IEnumerable<InvoiceJob> Jobs { get; set; }

        public IEnumerable<InvoiceMaterial> Materials { get; set; }

        public IEnumerable<InvoiceProduct> Products { get; set; }
    }
}