using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = [];
}
