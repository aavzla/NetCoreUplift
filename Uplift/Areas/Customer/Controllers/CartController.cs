using System;
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
            AddServicesToVM();
            return View(CartVM);
        }

        private void AddServicesToVM()
        {
            if (HttpContext.Session.GetObject<List<int>>(StaticDetails.SessionCart) != null)
            {
                List<int> sessionList = HttpContext.Session.GetObject<List<int>>(StaticDetails.SessionCart);
                if (CartVM.Services == null)
                {
                    CartVM.Services = new List<Service>();
                }

                foreach (int serviceId in sessionList)
                {
                    CartVM.Services.Add(_unitOfWork.Services.GetFirstOrDefault(u => u.Id == serviceId, "Frequency,Category"));
                }
            }
        }

        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = HttpContext.Session.GetObject<List<int>>(StaticDetails.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(StaticDetails.SessionCart, sessionList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            AddServicesToVM();
            return View(CartVM);
        }

        [HttpPost, ActionName(nameof(Summary))]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            AddServicesToVM();

            if (ModelState.IsValid)
            {
                CartVM.OrderInfo.OrderDate = DateTime.Now;
                CartVM.OrderInfo.Status = StaticDetails.OrderStatusSubmitted;
                CartVM.OrderInfo.ServiceCount = CartVM.Services.Count;
                _unitOfWork.OrderInfos.Add(CartVM.OrderInfo);
                _unitOfWork.Save();

                foreach (var service in CartVM.Services)
                {
                    OrderDetails orderDetails = new OrderDetails
                    {
                        ServiceId = service.Id,
                        OrderInfoId = CartVM.OrderInfo.Id,
                        ServiceName = service.Name,
                        Price = service.Price
                    };
                    _unitOfWork.OrderDetails.Add(orderDetails);
                }
                _unitOfWork.Save();
            }
            else
            {
                return View(CartVM);
            }

            HttpContext.Session.SetObject(StaticDetails.SessionCart, new List<int>());
            return RedirectToAction(nameof(OrderConfirmation), new { id = CartVM.OrderInfo.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}