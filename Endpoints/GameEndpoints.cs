using System;
using Store.Api.Data;
using Store.Api.Dtos;
using Store.Api.Entities;
using Store.Api.Mapping;
namespace Store.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDtos> games = [
    new (1,"Street Fighter", "fighting", 19.99M, new DateOnly(1992, 7,15)),
    new (2,"Minecraft", "Open World", 9.99M, new DateOnly(2000, 1,10)),
    new (3,"CSGO", "Shoot", 1.99M, new DateOnly(2025, 3,11))
   ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games");
        //Get /games/id
        group.MapGet("/{id}", (int id) => games.Find(game => game.Id == id))
        .WithName(GetGameEndpointName);

        //Get /game/id
        group.MapGet("/", () => games);

        //POST /game
        group.MapPost("/", (CreateGameDtos newGame, GameStoreContext dbContext) =>{
            Game game = newGame.ToEntity();

            game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);

            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName,
            new { id = game.Id },
            game.ToDto());
    });

        //PUT //game/id
        group.MapPut("/{id}", (int id, UpdateGameDtos updatedGame) =>{
            var index = games.FindIndex(game => game.Id == id);

            games[index] = new GameDtos(
            id,
            updatedGame.Name,
            updatedGame.Genre,
            updatedGame.Price,
            updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //Delete /game/id
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
        return group;
    }
}