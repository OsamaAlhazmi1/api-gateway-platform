using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiDefinitionDTOs;

public record CreateApiClientDTO
(
    [Required][StringLength(100)]string Name ,
    [Required][StringLength(100)]string ApiKey,
    [Required]bool IsActive,
    [Required] DateTime CreatedAt


);