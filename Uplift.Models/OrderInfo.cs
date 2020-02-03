using System;
using System.ComponentModel.DataAnnotations;

namespace Uplift.Models
{
    public class OrderInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required, Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string Comments { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        [Display(Name = "Count")]
        /// <summary>
        /// Total Count of services requested
        /// </summary>
        public int ServiceCount { get; set; }

    }
}
