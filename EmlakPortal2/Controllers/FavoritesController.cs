using EmlakPortal2.Data;
using EmlakPortal2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmlakPortal2.Controllers
{
    [Authorize] // Sadece giriş yapanlar favori ekleyebilir
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. KULLANICININ FAVORİLERİNİ LİSTELEME
        public IActionResult Index()
        {
            // Şu anki giriş yapmış kullanıcının ID'sini bul
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = _context.Favorites
                .Include(f => f.Property) // İlan detaylarını getir
                .ThenInclude(p => p.Category) // Kategori adını da getir
                .Include(f => f.Property.PropertyImages) // Resmini de getir
                .Where(f => f.UserId == userId)
                .Select(f => f.Property) // Bize sadece Property listesi lazım
                .ToList();

            return View(favorites);
        }

        // 2. FAVORİYE EKLE / ÇIKAR (Toggle)
        [HttpPost]
        public IActionResult Toggle(int propertyId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Önce veritabanına bak: Bu kullanıcı bu ilanı zaten favorilemiş mi?
            var existingFav = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId && f.PropertyId == propertyId);

            if (existingFav != null)
            {
                // Zaten varsa FAVORİDEN ÇIKAR
                _context.Favorites.Remove(existingFav);
                _context.SaveChanges();
                return Json(new { success = true, status = "removed" });
            }
            else
            {
                // Yoksa FAVORİYE EKLE
                var newFav = new Favorite
                {
                    UserId = userId,
                    PropertyId = propertyId
                };
                _context.Favorites.Add(newFav);
                _context.SaveChanges();
                return Json(new { success = true, status = "added" });
            }
        }
    }
}