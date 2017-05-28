using System;

namespace Fodraszat.Models
{
    public class InvoiceListItem
    {
        public int InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        public int Price { get; set; }

        public string Client { get; set; }
    }
}