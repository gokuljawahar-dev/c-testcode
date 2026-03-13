namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels.FeedbackResponseViewModel;

public class TopicFeedbackResponseViewModelValidator
    : AbstractValidator<TopicFeedbackResponseViewModel>
{
    public TopicFeedbackResponseViewModelValidator()
    {
        this.RuleFor(feedback => feedback.TopicFeedbackQuestionId)
            .NotEmpty()
            .WithMessage("TopicFeedbackQuestionId is required.");

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
