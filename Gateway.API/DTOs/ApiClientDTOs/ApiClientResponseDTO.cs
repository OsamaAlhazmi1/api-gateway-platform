using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiDefinitionDTOs;

public record ApiClientResponseDTO
(
    [Required] int Id, 
    [Required]string Name ,
    [Required]string ApiKey,
    [Required]bool IsActive,
    [Required] DateTime CreatedAt


);