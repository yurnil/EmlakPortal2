using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmlakPortal2.Repositories.Abstract; // IUnitOfWork için

namespace EmlakPortal2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _uow;

        public DashboardController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            // İstatistikleri toplayalım
            ViewBag.KategoriSayisi = _uow.Category.GetAll().Count();
            ViewBag.IlanSayisi = _uow.Property.GetAll().Count();

            // Eğer User tablosuna erişim repository'de yoksa şimdilik sadece bunları saydırıyoruz.

            return View();
        }
    }
}