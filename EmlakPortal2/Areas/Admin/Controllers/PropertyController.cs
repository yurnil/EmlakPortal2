using EmlakPortal2.Data; 
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ApplicationDbContext _context;

        public PropertyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _context = context; 
        }

        public IActionResult Index()
        {
            var propertyList = _unitOfWork.Property.GetAll(includeProperties: "Category");
            return View(propertyList);
        }

        // GET: Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ViewBag.CategoryList = categoryList;
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Property property, List<IFormFile> files)
        {
            ModelState.Remove("AppUser");
            ModelState.Remove("Category");
            ModelState.Remove("PropertyImages");

            bool isAjax = string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase);

            if (ModelState.IsValid)
            {
                _unitOfWork.Property.Add(property);
                _unitOfWork.Save();

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

                if (isAjax)
                {
                    return Json(new { success = true, message = "İlan başarıyla oluşturuldu.", redirect = Url.Action("Index") });
                }

                return RedirectToAction("Index");
            }

            if (isAjax)
            {
                var errors = ModelState.Where(kvp => kvp.Value.Errors.Count >0)
                .Select(kvp => new {
                    key = kvp.Key,
                    errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                }).ToArray();
                return BadRequest(new { success = false, message = "Doğrulama hatası.", errors });
            }

            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = categoryList;
            return View(property);
        }

        // DELETE
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


        // Edit GET
        public IActionResult Edit(int id)
        {
            var property = _context.Properties.Find(id);

            if (property == null) return NotFound();

            ViewBag.Categories = new SelectList(_unitOfWork.Category.GetAll(), "Id", "Name", property.CategoryId);
            return View(property);
        }

        // Edit POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Property model)
        {
            // 1. Veritabanındaki gerçek veriyi bul
            var dbItem = _context.Properties.Find(model.Id);

            if (dbItem == null) return NotFound();

            // 2. Değişiklikleri elle işle
            dbItem.Title = model.Title;
            dbItem.Price = model.Price;
            dbItem.Description = model.Description;
            dbItem.CategoryId = model.CategoryId;
            dbItem.RoomCount = model.RoomCount;
            dbItem.IsitmaTipi = model.IsitmaTipi;
            dbItem.EsyaliMi = model.EsyaliMi;
            dbItem.BulunduguKat = model.BulunduguKat;
            dbItem.BinaYasi = model.BinaYasi;
            dbItem.KatSayisi = model.KatSayisi;
            dbItem.BalkonSayisi = model.BalkonSayisi;
            dbItem.KullanimDurumu = model.KullanimDurumu;
            dbItem.SiteIcerisinde = model.SiteIcerisinde;


            try
            {
                _context.SaveChanges();
                TempData["success"] = "İlan ZORLA güncellendi! :)";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("HATA OLUŞTU: " + ex.Message);
            }
        }
    }
}