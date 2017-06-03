using Fodraszat.Models.InventoryEntities;
using System.Collections.Generic;

namespace Fodraszat.Models.ResponseObjects
{
    public class InventoryResponse
    {
        public IEnumerable<InventoryMaterial> Materials { get; set; }

        public IEnumerable<InventoryProduct> Products { get; set; }
    }
}