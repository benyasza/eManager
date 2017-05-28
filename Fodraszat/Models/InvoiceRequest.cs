using System.ComponentModel.DataAnnotations;

namespace Fodraszat.Models
{
    public class InvoiceRequest
    {
        [Required]
        public string SelectedHairDressers { get; set; }

        [Required]
        public string SelectedJobs { get; set; }

        public string SelectedMaterials { get; set; }

        public string Client { get; set; }
    }
}