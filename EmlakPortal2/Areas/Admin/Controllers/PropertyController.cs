using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmlakPortal2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PropertyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // Dosya işlemleri için

        public PropertyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // Kategorisiyle beraber tüm ilanları getir (Include)
            var propertyList = _unitOfWork.Property.GetAll(includeProperties: "Category");
            return View(propertyList);
        }

        // GET: Create
        public IActionResult Create()
        {
            // Dropdown için Kategorileri hazırla
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ViewBag.CategoryList = categoryList;
            return View();
        }

        // POST: Create İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Property property, List<IFormFile> files)
        {
            // --- BU KISIM HAYAT KURTARIR ---
            // İlişkili tabloların boş gelmesini sorun etme diyoruz:
            ModelState.Remove("AppUser");
            ModelState.Remove("Category");
            ModelState.Remove("PropertyImages");
            // -------------------------------

            if (ModelState.IsValid)
            {
                // 1. İlanı Kaydet
                _unitOfWork.Property.Add(property);
                _unitOfWork.Save();

                // 2. Resimleri Yükle ve Kaydet
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\properties");

                        if (!Directory.Exists(productPath)) Directory.CreateDirectory(productPath);

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        PropertyImage propertyImage = new PropertyImage()
                        {
                            ImageUrl = @"\images\properties\" + fileName,
                            PropertyId = property.Id,
                        };
                        _unitOfWork.PropertyImage.Add(propertyImage);
                    }
                    _unitOfWork.Save();
                }

                TempData["success"] = "İlan başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                // HATA VARSA GÖRELİM:
                // Hangi alanın hatalı olduğunu anlamak için hata ayıklama satırı:
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                // (İstersen buraya breakpoint koyup errors değişkenine bakabilirsin)
            }

            // Hata varsa dropdown tekrar dolsun
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = categoryList;

            return View(property);
        }

        // SİLME İŞLEMİ (Basit Hali)
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var obj = _unitOfWork.Property.GetById(id.Value);
            if (obj == null) return NotFound();
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            var obj = _unitOfWork.Property.GetById(id);
            if (obj == null) return NotFound();

            _unitOfWork.Property.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "İlan silindi.";
            return RedirectToAction("Index");
        }
    }
}