using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/formation";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "asp-backend",
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            NameClaimType = "preferred_username"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireUserRole", policy =>
        policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "ROLE_USER"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
//app.UseSwagger();
//app.UseSwaggerUI();

app.MapGet("/protected", (ClaimsPrincipal user) =>
{
    var claims = user.Claims.Select(c => $"{c.Type} = {c.Value}");
    Console.WriteLine("Claims:");
    foreach (var claim in claims)
    {
        Console.WriteLine(claim);
    }
    return Results.Ok($"Bienvenue {user.Identity?.Name}");
}).RequireAuthorization("RequireUserRole");

app.Run();
