using FluentValidation;
using GameBacklog.Dtos;

namespace GameBacklog.Validation;

public class GameDtoValidator : AbstractValidator<GameDto>
{
    public GameDtoValidator()
    {
        RuleFor(game => game.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(game => game.Rating)
            .InclusiveBetween(0, 10)
            .WithMessage("Rating must be between 0 and 10.");
    }
}