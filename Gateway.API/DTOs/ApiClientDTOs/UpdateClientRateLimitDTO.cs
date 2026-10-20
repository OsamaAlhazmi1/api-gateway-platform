using System.ComponentModel.DataAnnotations;
namespace Gateway.API.DTOs.ApiClientDTOs;


public record UpdateClientRateLimitDTO
(
    [Required][Range(1,1000)]int RateLimitPerMinute 

);