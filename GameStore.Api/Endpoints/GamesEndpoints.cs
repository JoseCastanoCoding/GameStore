using System;
using GameStore.Api.DTOs;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
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

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
            .WithParameterValidation();

        group.MapGet("/", () => games);

        // GET GAMES

        group.MapGet("/{id}", (int id) => {

            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);

            }).WithName(GetGameEndpointName);

        // POST Games

        group.MapPost("/", (CreateGameDto newGame) => {
            
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

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {
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

        group.MapDelete("/{id}", (int id) => {

            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
