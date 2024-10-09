using System.Runtime.CompilerServices;
using GameStore.Api.DTOs;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "X-COM 1",
        "Science fiction",
        25.65m,
        new DateOnly(1999, 2, 14)),
    new (
        2,
        "X-COM 2",
        "Science fiction",
        35.65m,
        new DateOnly(2002, 7, 15)),
    new (
        3,
        "Final Fantasy XV",
        "Roleplaying",
        28.54m,
        new DateOnly(2000, 1, 7))
];

app.MapGet("games", () => games);

// GET GAMES

app.MapGet("games/{id}", (int id) => {

    GameDto? game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);

    }).WithName(GetGameEndpointName);

// POST Games

app.MapPost("games", (CreateGameDto newGame) => {
    
    GameDto game = new(

        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new{ id = game.Id }, game);
});

// Update

app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) => {
    var gameIndex = games.FindIndex(game => game.Id == id);

    if (gameIndex == -1) {
        return Results.NotFound();
    } 

    games[gameIndex] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// Delete

app.MapDelete("games/{id}", (int id) => {

    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.MapGet("/", () => "Hello World!");

app.Run();
