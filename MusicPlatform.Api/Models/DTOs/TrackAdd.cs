namespace MusicPlatform.Api.Models.DTOs;

public record TrackAdd
{
    public string Title { get; set; } = null!;
    public int GenreId { get; set; }
    public IFormFile TrackFile { get; set; } = null!;
    public IFormFile CoverFile { get; set; } = null!;
}
