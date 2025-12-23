using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmlakPortal2.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
 
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Tüm kategorileri getir
            var categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }

        // GET: Create Page
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save(); // Veritabanına işle
                TempData["success"] = "Kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Edit Page
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = _unitOfWork.Category.GetById(id.Value);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Edit Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Delete Page
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = _unitOfWork.Category.GetById(id.Value);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Delete Action
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            var category = _unitOfWork.Category.GetById(id);
            if (category == null) return NotFound();

            _unitOfWork.Category.Delete(category);
            _unitOfWork.Save();
            TempData["success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}