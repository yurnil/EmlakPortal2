using Microsoft.AspNetCore.Identity;

namespace EmlakPortal2.Models
{

    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

}
