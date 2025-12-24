using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmlakPortal2.Repositories.Abstract; 

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
            ViewBag.KategoriSayisi = _uow.Category.GetAll().Count();
            ViewBag.IlanSayisi = _uow.Property.GetAll().Count();


            return View();
        }
    }
}