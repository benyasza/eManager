using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fodraszat.Models
{
    public class InvoiceDetails
    {
        public int InvoiceNumber { get; set; }

        public int TotalCost { get; set; }

        public IEnumerable<InvoiceDetailsRow> Jobs { get; set; }

        public IEnumerable<InvoiceDetailsRow> Materials { get; set; }
    }
}