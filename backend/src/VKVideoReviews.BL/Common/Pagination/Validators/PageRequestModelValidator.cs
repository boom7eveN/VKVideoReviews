using FluentValidation;

namespace VKVideoReviews.BL.Common.Pagination.Validators;

public class PageRequestModelValidator : AbstractValidator<PageRequestModel>
{
    public const int MaxPageSize = 100;

    public PageRequestModelValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Номер страницы должен быть не меньше 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage($"Размер страницы должен быть от 1 до {MaxPageSize}");
    }
}