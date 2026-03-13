namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels.FeedbackResponseViewModel;

public class CourseFeedbackResponseViewModelValidator
    : AbstractValidator<CourseFeedbackResponseViewModel>
{
    public CourseFeedbackResponseViewModelValidator()
    {
        this.RuleFor(feedback => feedback.CourseFeedbackQuestionId)
            .NotEmpty()
            .WithMessage("CourseFeedbackQuestionId is required.");

        this.RuleFor(feedback => feedback.LearnerId)
            .NotEmpty()
            .WithMessage("LearnerId is required.");

        this.RuleFor(feedback => feedback.Response)
            .NotEmpty()
            .When(feedback => string.IsNullOrEmpty(feedback.OptionText))
            .WithMessage("Either Response or OptionText must be provided.");

        this.RuleFor(feedback => feedback.OptionText)
            .NotEmpty()
            .When(feedback => string.IsNullOrEmpty(feedback.Response))
            .WithMessage("Either Response or OptionText must be provided.");
    }
}
