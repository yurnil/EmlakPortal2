using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using EmlakPortal2.Hubs;

namespace EmlakPortal2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<GeneralHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IHubContext<GeneralHub> hubContext)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        // ANASAYFA (Hem normal açýlýþ hem de butonlar için kategorileri hazýrlar)
        public IActionResult Index()
        {
            // 1. Kategorileri View'a taþý (Butonlar için)
            var categories = _unitOfWork.Category.GetAll();
            ViewBag.Categories = categories;

            // 2. Ýlanlarý Getir
            var propertyList = _unitOfWork.Property.GetAll(includeProperties: "Category,PropertyImages");
            return View(propertyList);
        }

        // AJAX ÝÇÝN FÝLTRELEME METODU
        [HttpGet]
        public IActionResult FilterProperties(int? categoryId)
        {
            IEnumerable<Property> propertyList;

            if (categoryId == null || categoryId == 0)
            {
                // Kategori seçilmediyse hepsini getir
                propertyList = _unitOfWork.Property.GetAll(includeProperties: "Category,PropertyImages");
            }
            else
            {
                // Seçilen kategoriye göre filtrele
                propertyList = _unitOfWork.Property.GetAll(u => u.CategoryId == categoryId, includeProperties: "Category,PropertyImages");
            }

            return PartialView("_PropertyListPartial", propertyList);
        }

        // DETAY SAYFASI
        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();

            var property = _unitOfWork.Property
                .GetAll(u => u.Id == id, includeProperties: "Category,PropertyImages")
                .FirstOrDefault();

            if (property == null) return NotFound();

            // --- SÝNYAL GÖNDER ---
            // "ReceiveMessage" adýndaki dinleyiciye mesaj yolluyoruz
            _hubContext.Clients.All.SendAsync("ReceiveMessage", "Sistem", $"{property.Title} ilanýna biri bakýyor!");
            // ---------------------

            return View(property);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}