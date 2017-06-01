using Fodraszat.Models.RequestObjects.DataObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fodraszat.Models.RequestObjects
{
    public class InvoiceRequest
    {
        [Required]
        public IEnumerable<JobObject> Jobs { get; set; }

        public IEnumerable<MaterialObject> Materials { get; set; }

        public IEnumerable<PurchasedProductObject> Products { get; set; }

        public string Client { get; set; }
    }
}