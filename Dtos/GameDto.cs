using System.ComponentModel.DataAnnotations;

namespace GameBacklog.Dtos;

public record GameDto(
string Name,
string Genre,
string Platform,
double? Rating
);