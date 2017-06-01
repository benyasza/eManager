using System;

namespace Fodraszat.Models
{
    public class InvoiceListItem
    {
        public int InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        public int TotalPrice { get; set; }

        public int PriceWithDiscount { get; set; }

        public string Client { get; set; }
    }
}