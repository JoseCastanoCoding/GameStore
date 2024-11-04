using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public record class CreateGameDto(
    [Required][StringLength(100)] string Name, 
    [Required][StringLength(30)] string Genre, 
    [Range(1, 100)]decimal Price, 
    DateOnly ReleaseDate);