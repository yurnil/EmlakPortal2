using EmlakPortal2.Data;
using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;
using EmlakPortal2.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmlakPortal2.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity Ayarları (BİZİM YAZDIĞIMIZ - Scaffolding'in eklediğini sildik)
// NOT: Scaffolding bazen buraya 'AddDefaultIdentity' ekler, o yüzden hata verir.
// Biz kendi 'AddIdentity' kodumuzu koruyoruz.
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. Cookie Ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Scaffolding sonrası yol değişti!
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// 4. Repository Pattern Tanımları
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 5. MVC ve Razor Pages Servisleri
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Login/Register sayfaları için şart
builder.Services.AddSignalR();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// Pipeline Ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik Doğrulama
app.UseAuthorization();  // Yetkilendirme

// Rotalar
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Login sayfalarının çalışması için şart
app.MapHub<EmlakPortal2.Hubs.GeneralHub>("/general-hub");
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Member" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();