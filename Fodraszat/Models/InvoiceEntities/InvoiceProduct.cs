using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Services;

namespace Fodraszat.Models.InvoiceEntities
{
    public class InvoiceProduct
    {
        public InvoiceProduct(PurchasedProductObject productObject)
        {
            if (productObject != null)
            {
                Name = DataService.GetProductById(productObject.Id)?.Name;
                Quantity = productObject.Quantity;
                Price = productObject.Price;
            }
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}