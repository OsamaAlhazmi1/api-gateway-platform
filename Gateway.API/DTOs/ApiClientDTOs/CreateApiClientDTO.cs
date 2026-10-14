using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiDefinitionDTOs;

public record CreateApiClientDTO
(
    [Required][StringLength(100)]string Name 


);