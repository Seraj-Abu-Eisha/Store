using System;
using Store.Api.Dtos;
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
    group.MapGet("/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);
    group.MapGet("/", () => games);
    group.MapPost("/", (CreateGameDtos newGame) =>
{
    GameDtos game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

group.MapPut("/{id}", (int id, UpdateGameDtos updatedGame) =>
{
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

group.MapDelete("/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});


        return group;
    }
}