using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uplift.Models
{
    public class OrderDetails
    {
        //The key DataAnnotation is placed by AspNetCore automatically if the property follow the syntax of ending by Id or named Id only.
        public int Id { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int OrderInfoId { get; set; }
        [Required]
        public int ServiceId { get; set; }


        [ForeignKey(nameof(OrderInfoId))]
        public OrderInfo OrderInfo { get; set; }

        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; }
    }
}
