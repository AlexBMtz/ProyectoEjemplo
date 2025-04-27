using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.DTO.OutputDTO;

public class UserOutputDTO
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

}
