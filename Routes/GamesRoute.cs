using GameBacklog.Data;
using GameBacklog.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameBacklog.Routes;

public static class GamesRoute
{
    public enum Platform
    {
        PS5,
        Xbox,
        Nintendo,
        PC,
        Mobile
    }

    public static void MapGamesRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("/", async (
            GameBacklogContext dBcontext,
            [FromQuery] Platform? platform
        ) =>
        {
            string? platformName = platform?.ToString();

            return await dBcontext.Games
                .Where(g => platformName == null || g.Platform == platformName)
                .Select(g => new GameDetailsDto(
                    g.Id, g.Name, g.Genre, g.Platform, g.Rating
                ))
                .AsNoTracking()
                .ToListAsync();
        });

        group.MapGet("/{id}", async Task<Results<Ok<GameDetailsDto>, ProblemHttpResult>> (int id, GameBacklogContext dBcontext) =>
        {
            Game? game = await dBcontext.Games.FindAsync(id);

            if (game is null)
            {
                return TypedResults.Problem(detail: $"Game with id {id} not found", statusCode: 404, title: "Game not found");
            }

            GameDetailsDto gameDto = new GameDetailsDto(game.Id, game.Name, game.Genre, game.Platform, game.Rating);

            return TypedResults.Ok(gameDto);
        }).WithName("GetGame").ProducesProblem(404);

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

            return TypedResults.CreatedAtRoute(
                    value: gameDto,
                    routeName: "GetGame",
                    routeValues: new { id = game.Id }
                );
        });

        group.MapPut("/{id}", async Task<Results<Ok<GameDetailsDto>, NotFound>> (int id, GameDto updatedGame, GameBacklogContext dBcontext) =>
        {
            Game? game = await dBcontext.Games.FindAsync(id);
            if (game is null)
            {
                return TypedResults.NotFound();
            }
            game.Name = updatedGame.Name;
            game.Genre = updatedGame.Genre;
            game.Platform = updatedGame.Platform;
            game.Rating = updatedGame.Rating;

            await dBcontext.SaveChangesAsync();

            GameDetailsDto gameDto = new GameDetailsDto(game.Id, game.Name, game.Genre, game.Platform, game.Rating);

            return TypedResults.Ok(gameDto);
        });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, GameBacklogContext dBcontext) =>
        {
            Game? game = await dBcontext.Games.FindAsync(id);
            if (game is null)
            {
                return TypedResults.NotFound();
            }
            dBcontext.Games.Remove(game);
            await dBcontext.SaveChangesAsync();
            return TypedResults.NoContent();
        });
    }
}
