using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiDefinitionDTOs;

public record UpdateApiDefinitionDTO
(
    [Required][StringLength(100)]string Name ,
    [Required][StringLength(100)]string RoutePrefix,
    [Required][StringLength(100)]string DownstreamPath,
    [Required][Url]string DestinationAddress

);