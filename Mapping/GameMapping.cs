using Store.Api.Dtos;
using Store.Api.Entities;

namespace Store.Api.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDtos game)
    {
        return new Game()
        {
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static GameDtos ToDto(this Game game)
    {
         return new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            );

    }
}