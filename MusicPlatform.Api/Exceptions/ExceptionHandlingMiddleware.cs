using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MusicPlatform.Api.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException e)
        {
            await SendError(context, e);            
        }
        catch (AuthException e)
        {
            await SendError(context, e);
        }
    }

    private async Task SendError(HttpContext context, Exception e)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        ProblemDetails problem = new()
        {
            Status = (int)HttpStatusCode.BadRequest,
            Detail = e.Message
        };

        var json = JsonSerializer.Serialize(problem);

        await context.Response.WriteAsync(json);
    } 
}
