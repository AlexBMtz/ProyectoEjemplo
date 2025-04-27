namespace SharedModels.Models.DTO.OutputDTO;

public class UserTokenOutputDTO
{
    public string? TokenType { get; set; }
    public string? AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
}
