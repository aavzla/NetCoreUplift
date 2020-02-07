using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WebImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WebImageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            WebImage webImage = new WebImage();
            if (id.HasValue)
            {
                webImage = _unitOfWork.WebImages.GetFirstOrDefault(wi => wi.Id == id.Value);
                if (webImage == null)
                {
                    return NotFound();
                }
            }
            return View(webImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, WebImage webImage)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Any())
                {
                    byte[] picture = null;
                    using (var fs = files[0].OpenReadStream())
                    {
                        using (var ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            picture = ms.ToArray();
                        }
                    }
                    webImage.Picture = picture;
                }

                if (webImage.Id == 0)
                {
                    _unitOfWork.WebImages.Add(webImage);
                }
                else
                {
                    WebImage imageFromDB = _unitOfWork.WebImages.Get(id);
                    imageFromDB.Name = webImage.Name;
                    if (webImage.Picture != null)
                    {
                        imageFromDB.Picture = webImage.Picture;
                    }
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(webImage);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.WebImages.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            WebImage webImage = _unitOfWork.WebImages.Get(id);
            if (webImage == null)
            {
                return Json(new { success = false, message = "Error while deleting."} );
            }

            _unitOfWork.WebImages.Remove(webImage);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful."} );
        }

        #endregion
    }
}