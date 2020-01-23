using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Uplift.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required, Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required, Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
    }
}
