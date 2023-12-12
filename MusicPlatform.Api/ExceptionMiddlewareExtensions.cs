using MusicPlatform.Api.Exceptions;

namespace MusicPlatform.Api;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
