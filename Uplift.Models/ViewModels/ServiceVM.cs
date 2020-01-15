using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Uplift.Models.ViewModels
{
    public class ServiceVM
    {
        public Service Service { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FrequencyList { get; set; }
    }
}
