using Store.Api.Dtos;
using Store.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
List<GameDtos> games = [
    new (1,"Street Fighter", "fighting", 19.99M, new DateOnly(1992, 7,15)),
    new (2,"Minecraft", "Open World", 9.99M, new DateOnly(2000, 1,10)),
    new (3,"CSGO", "Shoot", 1.99M, new DateOnly(2025, 3,11))
];

app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName("GetGame");
app.MapGet("games", () => games);
app.MapPost("games", (CreateGameDtos newGame) =>
{
    GameDtos game = new(games.Count + 1, newGame.Name, newGame.Genre, newGame.Price, newGame.ReleaseDate);
    games.Add(game);
    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});

app.MapPut("games/{id}", (int id, UpdateGameDtos updatedGame) =>
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

app.Run(); 
