using GameBacklog.Data;
using GameBacklog.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameBacklog.Routes;

public static class GamesRoute
{
    public static void MapGamesRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("/", async (GameBacklogContext dBcontext) =>
                                                                    await dBcontext.Games
                                                                    .Select(g => new GameDetailsDto(g.Id, g.Name, g.Genre, g.Platform, g.Rating))
                                                                    .AsNoTracking()
                                                                    .ToListAsync());

        group.MapGet("/{id}", async (int id, GameBacklogContext dBcontext) =>
        {
            Game? game = await dBcontext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            GameDetailsDto gameDto = new GameDetailsDto(game.Id, game.Name, game.Genre, game.Platform, game.Rating);
            return Results.Ok(gameDto);
        }).WithName("GetGame");

        group.MapPost("/", async (GameDto newGame, GameBacklogContext dBcontext) =>
        {
            Game game = new Game {
                Name = newGame.Name,
                Genre = newGame.Genre,
                Platform = newGame.Platform,
                Rating = newGame.Rating
            };

            await dBcontext.Games.AddAsync(game);
            await dBcontext.SaveChangesAsync();

            GameDetailsDto gameDto = new GameDetailsDto(game.Id, game.Name, game.Genre, game.Platform, game.Rating);

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, gameDto);
        });

        group.MapPut("/{id}", async (int id, GameDto updatedGame, GameBacklogContext dBcontext) =>
        {
            Game? game = await dBcontext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            game.Name = updatedGame.Name;
            game.Genre = updatedGame.Genre;
            game.Platform = updatedGame.Platform;
            game.Rating = updatedGame.Rating;

            await dBcontext.SaveChangesAsync();

            GameDetailsDto gameDto = new GameDetailsDto(game.Id, game.Name, game.Genre, game.Platform, game.Rating);

            return Results.Ok(gameDto);
        });

        group.MapDelete("/{id}", async (int id, GameBacklogContext dBcontext) =>
        {
            Game? game = await dBcontext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            dBcontext.Games.Remove(game);
            await dBcontext.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
