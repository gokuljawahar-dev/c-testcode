namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.ViewModels.QuizQuestionViewModel;

public class QuestionOptionViewModelValidator : AbstractValidator<QuestionOptionViewModel>
{
    public QuestionOptionViewModelValidator()
    {
        this.RuleFor(option => option.Option).NotEmpty().WithMessage("Option text is required.");

        this.RuleFor(option => option.IsCorrect)
            .NotNull()
            .WithMessage("IsCorrect must be provided.");
    }
}
