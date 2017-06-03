using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Services;
using System.Collections.Generic;
using System.Linq;

namespace Fodraszat.Models.InventoryEntities
{
    public class InventoryProduct
    {
        public InventoryProduct(int id, IEnumerable<PurchasedProductObject> products)
        {
            if (id > 0 && (products?.Any() ?? false))
            {
                var product = DataService.GetProductById(id);

                Name = product?.Name;
                Quantity = products.Sum(t => t.Quantity);
            }
        }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}