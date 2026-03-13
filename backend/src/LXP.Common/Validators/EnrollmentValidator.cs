namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels;

public class EnrollmentValidator : AbstractValidator<EnrollmentViewModel>
{
    public EnrollmentValidator()
    {
        this.RuleFor(enroll => enroll.CourseId).NotEmpty().WithMessage("Course Name is required");

        this.RuleFor(enroll => enroll.LearnerId).NotEmpty().WithMessage("Learner Name is required");

        this.RuleFor(enroll => enroll.EnrollmentDate)
            .NotEmpty()
            .WithMessage("Enrollment Date is required");
    }
}
