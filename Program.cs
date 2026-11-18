using GameBacklog.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var games = new[]
{
    new GameDto("The Last of Us", "Action-Adventure", "PS4", 9.5),
    new GameDto("The Witcher 3: Wild Hunt", "Action-Adventure", "PS4", 9.5),
    new GameDto("The Elder Scrolls V: Skyrim", "Action-Adventure", "PS4", 9.5),
    new GameDto("The Legend of Zelda: Breath of the Wild", "Action-Adventure", "PS4", 9.5),
    new GameDto("The Legend of Zelda: Breath of the Wild", "Action-Adventure", "PS4", 9.5)
};

app.MapGet("/games", () => (games));

app.Run();