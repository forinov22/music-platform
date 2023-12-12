using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlatform.Api.Services;

namespace MusicPlatform.Api.Controllers;

[Authorize(Roles = "User")]
[Route("api/users")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }
}

