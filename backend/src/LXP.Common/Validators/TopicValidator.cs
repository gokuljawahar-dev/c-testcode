namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels;

public class CourseTopicViewModelValidator : AbstractValidator<CourseTopicViewModel>
{
    public CourseTopicViewModelValidator()
    {
        this.RuleFor(courseTopic => courseTopic.CourseId)
            .NotEmpty()
            .WithMessage("Course ID is required");

        this.RuleFor(courseTopic => courseTopic.Name).NotEmpty().WithMessage("Name is required");

        this.RuleFor(courseTopic => courseTopic.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        this.RuleFor(courseTopic => courseTopic.CreatedBy)
            .NotEmpty()
            .WithMessage("Created By is required");
    }
}
