using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiClientDTOs;

public record CreateApiClientDTO
(
    [Required][StringLength(100)]string Name 


);