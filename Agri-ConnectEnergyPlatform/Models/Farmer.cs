using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri_ConnectEnergyPlatform.Models
{
    public class Farmer
    {
        public int FarmerID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Role { get; set; }

        public ICollection<Product>? Products { get; set; }

        public string? IdentityUserId { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
