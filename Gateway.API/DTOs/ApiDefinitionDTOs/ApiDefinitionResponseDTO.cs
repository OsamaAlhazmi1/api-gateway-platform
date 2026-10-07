namespace Gateway.API.DTOs;

public record ApiDefinitionResponseDTO(
    int Id , 
    string Name ,
    string RoutePrefix,
    string DownstreamPath

);
