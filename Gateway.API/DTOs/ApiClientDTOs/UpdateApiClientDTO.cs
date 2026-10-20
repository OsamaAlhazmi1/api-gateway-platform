using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiClientDTOs;

public record UpdateApiClientDTO
(
    [Required][StringLength(100)]string Name ,
    [Required][StringLength(100)]string ApiKey,
    [Required]bool IsActive,
    [Required] DateTime CreatedAt


);