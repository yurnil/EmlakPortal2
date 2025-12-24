using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmlakPortal2.Areas.Admin.Controllers
{
 [Area("Admin")]
 [Authorize(Roles = "Admin")]
 public class FavoritesController : Controller
 {


 public IActionResult Index()
 {
 return RedirectToAction("Index", "Favorites", new { area = "" });
 }
 }
}
