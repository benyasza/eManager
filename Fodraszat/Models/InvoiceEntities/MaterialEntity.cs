using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Services;

namespace Fodraszat.Models.InvoiceEntities
{
    public class MaterialEntity
    {
        public MaterialEntity(MaterialObject materialObject)
        {
            if (materialObject != null)
            {
                Name = DataService.GetMaterialById(materialObject.Id)?.Name;
                Quantity = materialObject.Quantity;
                Price = materialObject.Price;
            }
        }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}