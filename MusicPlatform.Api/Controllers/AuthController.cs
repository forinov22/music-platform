using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Models;
using MusicPlatform.Api.Models.DTOs;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("artist/login")]
    public async Task<IActionResult> ArtistLogin([FromBody] LoginModel model)
    {
        var token = await authService.LoginArtist(model);
        if (token == null) {
            return Unauthorized("Wrong email or password");
        }
        return Ok(token);
    }

    [HttpPost("artist/register")]
    public async Task<IActionResult> ArtistRegister([FromBody] ArtistAdd dto)
    {
        var artist = await authService.RegisterArtist(dto);
        if (artist == null) {
            return BadRequest("This email is already taken");
        }
        return Ok(artist);
    }

    [HttpPost("user/login")]
    public async Task<IActionResult> UserLogin([FromBody] LoginModel model)
    {
        var token = await authService.LoginUser(model);
        if (token == null) {
            return Unauthorized("Wrong email or password");
        }
        return Ok(token);
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> UserRegister([FromBody] UserAdd dto)
    {
        var user = await authService.RegisterUser(dto);
        if (user == null) {
            return BadRequest("This email is already taken");
        }
        return Ok(user);
    }
}
