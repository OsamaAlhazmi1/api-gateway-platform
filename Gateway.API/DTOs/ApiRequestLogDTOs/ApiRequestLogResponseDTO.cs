namespace Gateway.API.DTOs.ApiRequestLogDTOs;

public record ApiRequestLogResponseDTO
(
    int ClinetId,
    string Method,
    string Path,
    long DurationMs,
    int StatusCode,
    DateTime TimestampUtc


);