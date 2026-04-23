using FluentValidation;
using VKVideoReviews.BL.Services.Reviews.Models;

namespace VKVideoReviews.BL.Services.Reviews.Validators;

public class UpdateReviewModelValidator : AbstractValidator<UpdateReviewModel>
{
    public UpdateReviewModelValidator()
    {
        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 10).WithMessage("Оценка должна быть от 1 до 10");

        RuleFor(x => x.Text)
            .NotEmpty()
            .MinimumLength(10).WithMessage("Отзыв должен быть минимум 10 символов")
            .MaximumLength(2000).WithMessage("Отзыв не должен превышать 2000 символов")
            .Matches(@"^\p{Lu}").WithMessage("Текст отзыва должен начинаться с заглавной буквы");
    }
}