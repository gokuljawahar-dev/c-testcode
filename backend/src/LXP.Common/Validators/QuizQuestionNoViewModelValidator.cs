namespace LXP.Common.Validators;

using FluentValidation;
using LXP.Common.Constants; // Add this namespace
using LXP.Common.ViewModels.QuizQuestionViewModel;

public class QuizQuestionNoViewModelValidator : AbstractValidator<QuizQuestionNoViewModel>
{
    public QuizQuestionNoViewModelValidator()
    {
        this.RuleFor(question => question.Question)
            .NotEmpty()
            .WithMessage("Question text is required.");

        this.RuleFor(question => question.QuestionType)
            .NotEmpty()
            .WithMessage("Question type is required.")
            .Must(this.BeAValidQuestionType)
            .WithMessage("Invalid question type.");

        this.RuleFor(question => question.Options)
            .NotEmpty()
            .WithMessage("Options are required.")
            .Must((model, options) => ValidateOptionsByQuestionType(model.QuestionType, options))
            .WithMessage("Invalid options for the given question type.");
    }

    private bool BeAValidQuestionType(string questionType) =>
        QuizQuestionTypes.MultiSelectQuestion.Equals(
            questionType,
            StringComparison.OrdinalIgnoreCase
        )
        || QuizQuestionTypes.MultiChoiceQuestion.Equals(
            questionType,
            StringComparison.OrdinalIgnoreCase
        )
        || QuizQuestionTypes.TrueFalseQuestion.Equals(
            questionType,
            StringComparison.OrdinalIgnoreCase
        );

    private static bool ValidateOptionsByQuestionType(
        string questionType,
        List<QuestionOptionViewModel> options
    ) =>
        questionType.ToUpper(System.Globalization.CultureInfo.CurrentCulture) switch
        {
            QuizQuestionTypes.MultiSelectQuestion
                => options.Count >= 5
                    && options.Count <= 8
                    && options.Count(o => o.IsCorrect) >= 2
                    && options.Count(o => o.IsCorrect) <= 3,
            QuizQuestionTypes.MultiChoiceQuestion
                => options.Count == 4 && options.Count(o => o.IsCorrect) == 1,
            QuizQuestionTypes.TrueFalseQuestion
                => options.Count == 2
                    && options.Count(o => o.IsCorrect) == 1
                    && options.Any(o => o.Option.Equals("true", StringComparison.OrdinalIgnoreCase))
                    && options.Any(o =>
                        o.Option.Equals("false", StringComparison.OrdinalIgnoreCase)
                    ),
            _ => false,
        };
}
