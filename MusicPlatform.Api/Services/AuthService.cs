using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Exceptions;
using MusicPlatform.Api.Models;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface IAuthService
{
    Task<UserDto> RegisterUser(UserAdd dto);
    Task<string> LoginUser(LoginModel model);
    Task<ArtistDto> RegisterArtist(ArtistAdd dto);
    Task<string> LoginArtist(LoginModel model);
}

public class AuthService : IAuthService
{
    private readonly MusicPlatformContext _context;
    private readonly IUserService userService;
    private readonly IArtistService artistService;
    private readonly ITokenService tokenService;

    public AuthService(IUserService userService, IArtistService artistService, ITokenService tokenService, MusicPlatformContext context)
    {
        this.userService = userService;
        this.artistService = artistService;
        this.tokenService = tokenService;
        _context = context;
    }

    public async Task<string> LoginArtist(LoginModel model)
    {
        var candidate = await _context.Artists.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (candidate == null || !BCrypt.Net.BCrypt.Verify(model.Password, candidate.PasswordHash))
            throw new AuthException(ExceptionMessages.WrongEmailOrPassword);

        var claims = new ClaimsIdentity(new [] {
            new Claim(ClaimTypes.NameIdentifier, candidate.Id.ToString()),
            new Claim(ClaimTypes.Email, candidate.Email),
            new Claim(ClaimTypes.Role, "Artist")
        });

        return tokenService.GenerateToken(claims);
    }

    public async Task<string> LoginUser(LoginModel model)
    {
        var candidate = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (candidate == null || !BCrypt.Net.BCrypt.Verify(model.Password, candidate.PasswordHash))
            throw new AuthException(ExceptionMessages.WrongEmailOrPassword);

        var claims = new ClaimsIdentity(new [] {
            new Claim(ClaimTypes.NameIdentifier, candidate.Id.ToString()),
            new Claim(ClaimTypes.Email, candidate.Email),
            new Claim(ClaimTypes.Role, "User")
        });

        return tokenService.GenerateToken(claims);
    }

    public async Task<ArtistDto> RegisterArtist(ArtistAdd dto)
    {
        var candidate = await _context.Artists.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (candidate != null)
            throw new AuthException(ExceptionMessages.EmailIsTaken);

        var artist = await artistService.CreateArtistAsync(dto);
        return artist;
    }

    public async Task<UserDto> RegisterUser(UserAdd dto)
    {
        var candidate = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (candidate != null)
            throw new AuthException(ExceptionMessages.EmailIsTaken);

        var user = await userService.CreateUserAsync(dto);
        return user;
    }
}
