using System.ComponentModel.DataAnnotations;

namespace Gateway.API.DTOs.ApiRequestLogDTOs;

public record CreateApiRequestLogDTO
(
    [Required]int ClinetId,
    [Required]string Method,
    [Required]string Path,
    [Required]int StatusCode,
    [Required]long DurationMs

);