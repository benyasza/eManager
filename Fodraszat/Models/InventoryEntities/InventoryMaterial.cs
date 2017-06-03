using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fodraszat.Models.InventoryEntities
{
    public class InventoryMaterial
    {
        public InventoryMaterial(int id, IEnumerable<MaterialObject> materials)
        {
            if (id > 0 && (materials?.Any() ?? false))
            {
                var material = DataService.GetMaterialById(id);

                Name = material?.Name;
                QuantiyPerBox = material?.QuantityPerBox ?? 0;

                Quantity = materials.Sum(t => t.Quantity);
                BoxCount = (int)Math.Ceiling((double)Quantity / QuantiyPerBox);
            }
        }

        public string Name { get; set; }

        public int QuantiyPerBox { get; set; }

        public int Quantity { get; set; }

        public int BoxCount { get; set; }
    }
}