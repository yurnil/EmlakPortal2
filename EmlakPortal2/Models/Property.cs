using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmlakPortal2.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty; // İlan Başlığı

        public string Description { get; set; } = string.Empty; // Açıklama

        [Required]
        public decimal Price { get; set; } // Fiyat

        [Required]
        public int RoomCount { get; set; } // Oda Sayısı

        [Required]
        public int Area { get; set; } // Metrekare

        public string Address { get; set; } = string.Empty; // Adres

        public bool IsSold { get; set; } = false; // Satıldı mı/Kiralandı mı?

        public DateTime CreatedDate { get; set; } = DateTime.Now; // İlan Tarihi

        // İlişkiler: Hangi Kategori?
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // İlişkiler: İlanı Ekleyen Kim?
        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        // Bir ilanın birden çok resmi olabilir
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    }
}