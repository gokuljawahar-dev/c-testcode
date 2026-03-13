namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels;

public class CourseCategoryViewModelValidator : AbstractValidator<CourseCategoryViewModel>
{
    public CourseCategoryViewModelValidator()
    {
        this.RuleFor(courseCategory => courseCategory.Category)
            .NotEmpty()
            .WithMessage("Category is required");

        this.RuleFor(courseCategory => courseCategory.CreatedBy)
            .NotEmpty()
            .WithMessage("CreatedBy is required");
    }
}
