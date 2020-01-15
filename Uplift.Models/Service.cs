using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uplift.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Service Name")]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int FrequencyId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        [ForeignKey(nameof(FrequencyId))]
        public Frequency Frequency { get; set; }
    }
}
