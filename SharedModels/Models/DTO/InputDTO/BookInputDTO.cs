using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.DTO.InputDTO
{
    public class BookInputDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PublishingYear { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public int EditorialId { get; set; }
    }
}
