namespace SharedModels.Models.DTO.OutputDTO
{
    public record BookOutputDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublishingYear { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public int EditorialId { get; set; }
        public string Editorial { get; set; }
    }
}
