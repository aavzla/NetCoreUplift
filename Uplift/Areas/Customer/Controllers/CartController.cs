using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public CartVM CartVM { get; set; }

        public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            CartVM = new CartVM()
            {
                OrderInfo = new OrderInfo(),
                Services = new List<Service>()
            };
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(StaticDetails.SessionCart) != null)
            {
                List<int> sessionList = HttpContext.Session.GetObject<List<int>>(StaticDetails.SessionCart);
                foreach (int serviceId in sessionList)
                {
                    CartVM.Services.Add(_unitOfWork.Services.GetFirstOrDefault(u => u.Id == serviceId, "Frequency,Category"));
                }
            }
            return View(CartVM);
        }

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = HttpContext.Session.GetObject<List<int>>(StaticDetails.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(StaticDetails.SessionCart, sessionList);
            return RedirectToAction(nameof(Index));
        }
    }
}