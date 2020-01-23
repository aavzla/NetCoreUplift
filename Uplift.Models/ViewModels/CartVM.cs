using System.Collections.Generic;

namespace Uplift.Models.ViewModels
{
    public class CartVM
    {
        public OrderInfo OrderInfo { get; set; }
        public IList<Service> Services { get; set; }
    }
}
