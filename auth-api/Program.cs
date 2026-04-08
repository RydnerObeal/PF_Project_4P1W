using System.Text;
using auth_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
using auth_api.Models;
using BCrypt.Net;
=======
>>>>>>> origin/iteration-5-rydner-obeal

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy
<<<<<<< HEAD
            .SetIsOriginAllowed(origin => origin != null && (origin.StartsWith("http://localhost") || origin.StartsWith("https://localhost")))
=======
            .WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174", "http://localhost:5175")
>>>>>>> origin/iteration-5-rydner-obeal
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

<<<<<<< HEAD
app.UseHttpsRedirection();
=======
>>>>>>> origin/iteration-5-rydner-obeal
app.UseCors("AllowWebApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

<<<<<<< HEAD
UserStore.Users.Add(new User
{
    Email = "admin@gmail.com",
    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
    Role = "admin"
});

UserStore.Users.Add(new User
{
    Email = "player@gmail.com",
    PasswordHash = BCrypt.Net.BCrypt.HashPassword("player123"),
    Role = "player"
});

=======
>>>>>>> origin/iteration-5-rydner-obeal
app.Run();