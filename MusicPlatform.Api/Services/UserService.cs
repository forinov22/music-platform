using BCrypt.Net;
using Mapster;
using Microsoft.EntityFrameworkCore;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Domains;
using MusicPlatform.Api.Models.DTOs;

namespace MusicPlatform.Api.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<UserDto> CreateUserAsync(UserAdd dto);
    Task<bool> DeleteUserAsync(int userId);
}


public class UserService : IUserService
{
    private readonly MusicPlatformContext _context;

    public UserService(MusicPlatformContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users.ProjectToType<UserDto>()
            .ToListAsync();
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.ProjectToType<UserDto>()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserDto> CreateUserAsync(UserAdd dto)
    {
        var user = dto.Adapt<User>();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user.Adapt<UserDto>();
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
