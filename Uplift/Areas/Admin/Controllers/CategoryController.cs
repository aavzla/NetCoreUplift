using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();

            if (id.HasValue)
            {
                category = _unitOfWork.Categories.Get(id.Value);

                if (category == null)
                {
                    return NotFound();
                }
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _unitOfWork.Categories.Add(category);
                }
                else
                {
                    _unitOfWork.Categories.Update(category);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Categories.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _unitOfWork.Categories.Get(id);
            if (objFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Categories.Remove(objFromDB);
            _unitOfWork.Save();
            return Json(new { success = true, message  = "Delete successfull." });
        }

        #endregion
    }
}