using System;

namespace Fodraszat.Models.RequestObjects
{
    public class InventoryRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}