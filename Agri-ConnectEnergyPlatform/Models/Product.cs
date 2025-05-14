using Agri_ConnectEnergyPlatform.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Agri_ConnectEnergyPlatform.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        public int FarmerID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [JsonIgnore] // prevents EF validation error
        public Farmer? Farmer { get; set; }
    }
}