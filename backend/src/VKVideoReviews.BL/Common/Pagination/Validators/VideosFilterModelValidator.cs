using FluentValidation;

namespace VKVideoReviews.BL.Common.Pagination.Validators;

public class VideosFilterModelValidator : AbstractValidator<VideosFilterModel>
{
    public VideosFilterModelValidator()
    {
        Include(new PageRequestModelValidator());

        RuleFor(x => x.TitlePart)
            .MaximumLength(200)
            .WithMessage("Поисковая строка не должна превышать 200 символов")
            .When(x => !string.IsNullOrEmpty(x.TitlePart));
    }
}