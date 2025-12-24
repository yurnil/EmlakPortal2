using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EmlakPortal2.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty; 

        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; } 
        [Required]
        public int RoomCount { get; set; } 
        [Required]
        public int Area { get; set; } 

        public string Address { get; set; } = string.Empty; 

        public bool IsSold { get; set; } = false; 

        public DateTime CreatedDate { get; set; } = DateTime.Now; 

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        [Display(Name = "Isıtma Tipi")]
        public string IsitmaTipi { get; set; } 

        [Display(Name = "Eşyalı mı?")]
        public bool EsyaliMi { get; set; }

        [Display(Name = "Bulunduğu Kat")]
        public int BulunduguKat { get; set; }

        [Display(Name = "Binanın Yaşı")]
        public int BinaYasi { get; set; }

        [Display(Name = "Toplam Kat Sayısı")]
        public int KatSayisi { get; set; }

        [Display(Name = "Balkon Sayısı")]
        public int BalkonSayisi { get; set; }

        [Display(Name = "Kullanım Durumu")]
        public string KullanimDurumu { get; set; } 

        [Display(Name = "Site İçerisinde mi?")]
        public bool SiteIcerisinde { get; set; }
    }
}