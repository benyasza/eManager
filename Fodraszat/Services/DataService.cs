using Fodraszat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fodraszat.Services
{
    public class DataService
    {
        private static HairDressEntities db = new HairDressEntities();

        public static HairDressers GetHairdresserById(int id)
        {
            return db.HairDressers.Find(id);
        }

        public static Jobs GetJobById(int id)
        {
            return db.Jobs.Find(id);
        }

        public static Materials GetMaterialById(int id)
        {
            return db.Materials.Find(id);
        }

        public static PurchasedProducts GetProductById(int id)
        {
            return db.PurchasedProducts.Find(id);
        }
    }
}