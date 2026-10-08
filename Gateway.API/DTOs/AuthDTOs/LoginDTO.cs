using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.AuthDTOs;

public record LoginDTO
(
    [Required] string Username,
    [Required] string Role
);
