using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicPlatform.Api;
using MusicPlatform.Api.Data;
using MusicPlatform.Api.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddNewtonsoftJson(opt =>
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection($"Jwt:{JwtOptions.TokenValidationParameters}"));

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("Jwt:TokenValidationParameters").Bind(jwtOptions);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
    options => {
        options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtOptions.ValidateIssuer,
                ValidIssuer = jwtOptions.ValidIssuer,
                ValidateAudience = jwtOptions.ValidateAudience,
                ValidAudience = jwtOptions.ValidAudience,
                ValidateLifetime = jwtOptions.ValidateLifetime,
                ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                IssuerSigningKey = jwtOptions.GetSecretKey()
            };
    });



var conStrBuilder = new NpgsqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("Postgres"));
conStrBuilder.Password = builder.Configuration["PostgresPassword"];
var connection = conStrBuilder.ConnectionString;

builder.Services.AddDbContext<MusicPlatformContext>(opt => {
    opt.UseNpgsql(connection);
});

builder.Services.AddMapster();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddCors();

var app = builder.Build();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors(builder => {
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.DbMigrate();

app.Run();
