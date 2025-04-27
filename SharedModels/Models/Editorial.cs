using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models
{
    public class Editorial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = [];
    }
}
