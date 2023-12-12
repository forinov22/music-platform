using Microsoft.EntityFrameworkCore;

namespace MusicPlatform.Api.Data;

public static class DataExtensions
{
    public static void DbMigrate(this WebApplication app) {
        var services = app.Services;
        using var scope = services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<MusicPlatformContext>();
        ctx.Database.Migrate();
    }
}
