namespace MusicPlatform.Api.Models.DTOs;

public record ArtistAdd
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Password { get; set; } = null!;
}
