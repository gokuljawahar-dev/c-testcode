namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels;
using Microsoft.AspNetCore.Http;

public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
{
    public CourseViewModelValidator()
    {
        this.RuleFor(course => course.Title).NotEmpty().WithMessage("Title is required");

        this.RuleFor(course => course.Level).NotEmpty().WithMessage("Level is required");

        this.RuleFor(course => course.Category).NotEmpty().WithMessage("Category is required");

        this.RuleFor(course => course.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        //RuleFor(course => course.Duration)
        //     .GreaterThan(0).WithMessage("Duration must be greater than 0")
        //     .Must(BeInDecimalForm).WithMessage("Duration must be in decimal form");

        this.RuleFor(course => course.Thumbnailimage)
            .Must(this.BeAValidSize)
            .WithMessage("Image must be less than 250kb")
            .Must(this.BeAValidFileType)
            .WithMessage("File must be jpeg or png");
    }

    private bool BeAValidSize(IFormFile file) =>
        // Assuming size is in bytes
        file.Length
        < 250 * 1024;

    private bool BeAValidFileType(IFormFile file)
    {
        var validFileTypes = new[] { "image/jpeg", "image/png" };
        return validFileTypes.Contains(file.ContentType);
    }
}
