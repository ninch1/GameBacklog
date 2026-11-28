using FluentValidation;
using GameBacklog.Data;
using GameBacklog.Dtos;
using GameBacklog.Middleware;
using GameBacklog.Routes;
using GameBacklog.Validation;
using Scalar.AspNetCore;
using GameBacklog.Authentication;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddValidation();
builder.Services.AddScoped<IValidator<GameDto>, GameDtoValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddGameBacklogDb();

builder.Services
    .AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyHandler>(
        "ApiKey",
        options => { });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapGamesRoutes();

app.MapGet("/whoami", (HttpContext context) =>
{
    return new
    {
        Name = context.User.Identity?.Name,
        Subscription = context.User.FindFirst("subscription")?.Value
    };
})
.RequireAuthorization();

app.MapGet("/premium", () => "Welcome to the premium endpoint!")
    .RequireAuthorization(policy => policy.RequireClaim("subscription", "gold"));

app.Run();