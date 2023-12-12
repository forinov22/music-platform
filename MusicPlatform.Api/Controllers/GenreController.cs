using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Models.DTOs;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Route("api/genres")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreService genreService;

    public GenreController(IGenreService genreService)
    {
        this.genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAllGenres()
    {
        var genres = await genreService.GetAllGenresAsync();
        return genres.ToList();
    }

    [HttpGet("{genreId}")]
    public async Task<ActionResult<GenreDto>> GetGenre(int genreId)
    {
        var genre = await genreService.GetGenreByIdAsync(genreId);
        return genre == null ? NotFound() : genre;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] GenreAdd dto)
    {
        var genre = await genreService.CreateGenreAsync(dto);
        return CreatedAtAction(nameof(GetGenre), new {genreId = genre.Id}, genre);
    }

    [HttpDelete("{genreId}")]
    public async Task<IActionResult> DeleteGenre([FromRoute] int genreId)
    {
        var deleted = await genreService.DeleteGenreAsync(genreId);
        return deleted ? NoContent() : NotFound();
    }
}
