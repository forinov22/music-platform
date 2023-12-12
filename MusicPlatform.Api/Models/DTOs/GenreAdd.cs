namespace MusicPlatform.Api.Models.DTOs;

public record GenreAdd
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}
