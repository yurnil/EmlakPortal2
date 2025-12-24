using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakPortal2.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        // Hangi Kullanıcı?
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        // Hangi İlan?
        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public Property Property { get; set; }
    }
}