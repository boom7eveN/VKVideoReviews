using FluentValidation;
using VKVideoReviews.BL.Services.Genres.Models;

namespace VKVideoReviews.BL.Services.Genres.Validators;

public class UpdateGenreModelValidator : AbstractValidator<UpdateGenreModel>
{
    public UpdateGenreModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название жанра обязательно")
            .MinimumLength(2).WithMessage("Название жанра слишком короткое")
            .MaximumLength(50).WithMessage("Название жанра слишком длинное")
            .Matches(@"^\p{Lu}").WithMessage("Жанр должен начинаться с заглавной буквы");
    }
}