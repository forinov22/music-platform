using Mapster;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Domains;
using MusicPlatform.Api.Exceptions;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllGenresAsync();
    Task<GenreDto> GetGenreByIdAsync(int genreId);
    Task<GenreDto> CreateGenreAsync(GenreAdd dto);
    Task<bool> DeleteGenreAsync(int genreId);
}

public class GenreService : IGenreService
{
    private readonly MusicPlatformContext context;

    public GenreService(MusicPlatformContext context)
    {
        this.context = context;
    }

    public async Task<GenreDto> CreateGenreAsync(GenreAdd dto)
    {
        var genre = dto.Adapt<Genre>();
        await context.Genres.AddAsync(genre);
        await context.SaveChangesAsync();
        return genre.Adapt<GenreDto>();
    }

    public async Task<bool> DeleteGenreAsync(int genreId)
    {
        var genre = await context.Genres.FindAsync(genreId);

        if (genre == null) return false;

        context.Genres.Remove(genre);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
    {
        return await context.Genres.ProjectToType<GenreDto>().ToListAsync();
    }

    public async Task<GenreDto> GetGenreByIdAsync(int genreId)
    {
        var genre = await context.Genres.ProjectToType<GenreDto>()
            .FirstOrDefaultAsync(g => g.Id == genreId);

        if (genre == null)
            throw new NotFoundException(ExceptionMessages.GenreNotFound);

        return genre;
    }
}
