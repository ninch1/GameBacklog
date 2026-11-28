using FluentValidation;
using GameBacklog.Dtos;

namespace GameBacklog.Validation;

public class GameDtoValidator : AbstractValidator<GameDto>
{
    public GameDtoValidator()
    {
        RuleFor(game => game.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        RuleFor(game => game.Genre)
            .NotEmpty()
            .WithMessage("Genre is required.")
            .MaximumLength(100)
            .WithMessage("Genre cannot exceed 100 characters.");

        RuleFor(game => game.Platform)
            .NotEmpty()
            .WithMessage("Platform is required.")
            .MaximumLength(100)
            .WithMessage("Platform cannot exceed 100 characters.");

        RuleFor(game => game.Rating)
            .NotNull()
            .WithMessage("Rating is required.")
            .InclusiveBetween(0, 10)
            .WithMessage("Rating must be between 0 and 10.");
    }
}