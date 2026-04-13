using FluentValidation;
using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Videos.Validators;

public class UpdateVideoModelValidator : AbstractValidator<UpdateVideoModel>
{
    private static readonly int MinYear = 1895;
    private static readonly int MaxYear = DateTime.UtcNow.Year + 5;

    public UpdateVideoModelValidator()
    {
        RuleFor(x => x.VideoUrl)
            .Must(BeAValidUrl!)
            .When(x => !string.IsNullOrWhiteSpace(x.VideoUrl))
            .WithMessage("Некорректный формат URL");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов")
            .Matches(@"^\p{Lu}").WithMessage("Название должно начинаться с заглавной буквы")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.ImageUrl)
            .Must(BeAValidUrl!)
            .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("Некорректный формат URL для изображения");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание слишком длинное")
            .Matches(@"^\p{Lu}").WithMessage("Описание должно начинаться с заглавной буквы")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.StartYear)
            .InclusiveBetween(MinYear, MaxYear)
            .When(x => x.StartYear.HasValue)
            .WithMessage($"Год начала должен быть между {MinYear} и {MaxYear}");

        RuleFor(x => x.EndYear)
            .LessThanOrEqualTo(MaxYear)
            .When(x => x.EndYear.HasValue)
            .WithMessage($"Год окончания не может быть больше {MaxYear}");

        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear!.Value)
            .When(x => x.EndYear.HasValue && x.StartYear.HasValue)
            .WithMessage("Год окончания не может быть раньше года начала");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}