using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View(new OrderInfo());
        }

        public IActionResult Details(int id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderInfo = _unitOfWork.OrderInfos.Get(id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(filter: od => od.OrderInfoId == id)
            };
            return View(orderVM);
        }

        public IActionResult Approve(int id)
        {
            var orderFromDB = _unitOfWork.OrderInfos.Get(id);
            if (orderFromDB == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderInfos.ChangeOrderStatus(id, StaticDetails.OrderStatusApproved);
            return View(nameof(Index));
        }

        public IActionResult Reject(int id)
        {
            var orderFromDB = _unitOfWork.OrderInfos.Get(id);
            if (orderFromDB == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderInfos.ChangeOrderStatus(id, StaticDetails.OrderStatusRejected);
            return View(nameof(Index));
        }

        #region API Calls

        public IActionResult GetAllOrders()
        {
            return Json(new { data = _unitOfWork.OrderInfos.GetAll() });
        }

        public IActionResult GetAllPendingOrders()
        {
            return Json(new { data = _unitOfWork.OrderInfos.GetAll( filter: x => x.Status == StaticDetails.OrderStatusSubmitted ) });
        }

        public IActionResult GetAllApprovedOrders()
        {
            return Json(new { data = _unitOfWork.OrderInfos.GetAll( filter: x => x.Status == StaticDetails.OrderStatusApproved ) });
        }

        #endregion
    }
}