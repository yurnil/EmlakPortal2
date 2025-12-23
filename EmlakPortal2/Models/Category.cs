using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EmlakPortal2.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;


        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}