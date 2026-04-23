using FluentValidation;
using VKVideoReviews.BL.Services.Videos.Models;

namespace VKVideoReviews.BL.Services.Videos.Validators;

public class CreateVideoModelValidator : AbstractValidator<CreateVideoModel>
{
    private static readonly int MinYear = 1895;
    private static readonly int MaxYear = DateTime.UtcNow.Year + 5;

    public CreateVideoModelValidator()
    {
        RuleFor(x => x.VideoUrl)
            .NotEmpty().WithMessage("Ссылка на видео обязательна")
            .Must(BeAValidUrl).WithMessage("Некорректный формат URL");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название видео обязательно")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов")
            .Matches(@"^\p{Lu}").WithMessage("Название должно начинаться с заглавной буквы");
        ;

        RuleFor(x => x.ImageUrl)
            .Must(BeAValidUrlOrEmpty).WithMessage("Некорректный формат URL для изображения");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание слишком длинное")
            .Matches(@"^\p{Lu}").WithMessage("Название должно начинаться с заглавной буквы");
        ;

        RuleFor(x => x.StartYear)
            .InclusiveBetween(MinYear, MaxYear)
            .WithMessage($"Год начала должен быть между {MinYear} и {MaxYear}");

        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear)
            .When(x => x.EndYear.HasValue)
            .WithMessage("Год окончания не может быть раньше года начала");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool BeAValidUrlOrEmpty(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return BeAValidUrl(url);
    }
}