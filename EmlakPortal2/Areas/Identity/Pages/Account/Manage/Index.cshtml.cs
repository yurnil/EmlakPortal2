using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using EmlakPortal2.Models;
using Microsoft.AspNetCore.Hosting; // Dosya işlemleri için
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmlakPortal2.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment; // Dosya yolu bulucu

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public string Username { get; set; }
        public string CurrentProfilePicture { get; set; } // Mevcut foto için

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefon Numarası")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Adınız")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Soyadınız")]
            public string Surname { get; set; }

            [Display(Name = "Profil Fotoğrafı")]
            public IFormFile ProfilePicture { get; set; } // Dosya yükleme kutusu
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            CurrentProfilePicture = user.ProfilePictureUrl; // Veritabanından fotoyu çek

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = user.Name,
                Surname = user.Surname
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // --- FOTOĞRAF YÜKLEME İŞLEMİ ---
            if (Input.ProfilePicture != null)
            {
                // 1. Klasör: wwwroot/images/profiles
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");

                // Klasör yoksa oluştur
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                // 2. İsim: avatar_RASTGELESAYI.jpg (Çakışmasın diye)
                string fileName = "avatar_" + Guid.NewGuid().ToString() + Path.GetExtension(Input.ProfilePicture.FileName);
                string filePath = Path.Combine(folder, fileName);

                // 3. Kaydet
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePicture.CopyToAsync(fileStream);
                }

                // 4. Veritabanına Yaz
                user.ProfilePictureUrl = "/images/profiles/" + fileName;
                await _userManager.UpdateAsync(user);
            }
            // ---------------------------------

            // Ad Soyad Güncelleme
            if (user.Name != Input.Name || user.Surname != Input.Surname)
            {
                user.Name = Input.Name;
                user.Surname = Input.Surname;
                await _userManager.UpdateAsync(user);
            }

            // Telefon Güncelleme
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profiliniz güncellendi";
            return RedirectToPage();
        }
    }
}