using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiClientDTOs;
public record ApiClientResponseDTO
(
    [Required] int Id, 
    [Required]string Name ,
    [Required]string ApiKey,
    [Required]bool IsActive,
    [Required]int RateLimit,
    [Required] DateTime CreatedAt


);