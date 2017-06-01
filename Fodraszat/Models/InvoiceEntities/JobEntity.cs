using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fodraszat.Models.InvoiceEntities
{
    public class JobEntity
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }
    }
}