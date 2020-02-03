using System.Collections.Generic;

namespace Uplift.Models.ViewModels
{
    public class OrderVM
    {
        public OrderInfo OrderInfo { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
