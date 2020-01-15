using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();

            if (id.HasValue)
            {
                frequency = _unitOfWork.Frequencies.Get(id.Value);

                if (frequency == null)
                {
                    return NotFound();
                }
            }

            return View(frequency);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                {
                    _unitOfWork.Frequencies.Add(frequency);
                }
                else
                {
                    _unitOfWork.Frequencies.Update(frequency);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Frequencies.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _unitOfWork.Frequencies.Get(id);
            if (objFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            _unitOfWork.Frequencies.Remove(objFromDB);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successfull" });
        }

        #endregion
    }
}