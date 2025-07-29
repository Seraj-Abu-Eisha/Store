namespace Store.Api.Dtos;

public record class CreateGameDtos (
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate);