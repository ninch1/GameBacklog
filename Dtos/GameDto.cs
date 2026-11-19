using System.ComponentModel.DataAnnotations;

namespace GameBacklog.Dtos;

public record GameDto(
[Required] [MaxLength(100)] string Name,
[Required] [MaxLength(100)] string Genre,
[Required] [MaxLength(100)] string Platform,
[Required] [Range(0, 10)] double Rating
);