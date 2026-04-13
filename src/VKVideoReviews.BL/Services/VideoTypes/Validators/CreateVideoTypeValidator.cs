using FluentValidation;
using VKVideoReviews.BL.Services.VideoTypes.Models;

namespace VKVideoReviews.BL.Services.VideoTypes.Validators;

public class CreateVideoTypeValidator : AbstractValidator<CreateVideoTypeModel>
{
    public CreateVideoTypeValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название типа видео обязательно")
            .MinimumLength(2).WithMessage("Название типа видео слишком короткое")
            .MaximumLength(50).WithMessage("Название типа видео слишком длинное")
            .Matches(@"^\p{Lu}").WithMessage("Тип видео должен начинаться с заглавной буквы");
    }
}