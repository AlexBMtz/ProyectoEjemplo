using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    public string PublishingYear { get; set; } = string.Empty;
    [Required]
    [ForeignKey("AuthorId")]
    public Author Author { get; set; }
    [Required]
    [ForeignKey("EditorialId")]
    public Editorial Editorial { get; set; }
}
