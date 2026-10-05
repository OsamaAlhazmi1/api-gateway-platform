using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiDefinitionDTOs;

public record CreateApiDefinitionDTOs
(
    [Required][StringLength(100)]string Name ,
    [Required][StringLength(100)]string RoutePrefix,
    [Required][Url]string DestinationAddress



);