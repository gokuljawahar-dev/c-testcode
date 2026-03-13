namespace LXP.Core.Services;

using LXP.Common.Constants;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class QuizQuestionService(IQuizQuestionRepository quizQuestionRepository)
    : IQuizQuestionService
{
    private readonly IQuizQuestionRepository _quizQuestionRepository = quizQuestionRepository;

    public async Task<Guid> AddQuestionAsync(
        QuizQuestionViewModel quizQuestion,
        List<QuestionOptionViewModel> options
    )
    {
        if (string.IsNullOrWhiteSpace(quizQuestion.Question))
        {
            throw new ArgumentException(
                "Question cannot be null or empty.",
                nameof(quizQuestion.Question)
            );
        }

        if (string.IsNullOrWhiteSpace(quizQuestion.QuestionType))
        {
            throw new ArgumentException(
                "QuestionType cannot be null or empty.",
                nameof(quizQuestion.QuestionType)
            );
        }

        quizQuestion.QuestionType = quizQuestion.QuestionType.ToUpper(
            System.Globalization.CultureInfo.CurrentCulture
        );

        if (!IsValidQuestionType(quizQuestion.QuestionType))
        {
            throw new ArgumentException(
                "Invalid question type.",
                nameof(quizQuestion.QuestionType)
            );
        }

        if (!ValidateOptions(quizQuestion.QuestionType, options))
        {
            throw new ArgumentException(
                "Invalid options for the given question type.",
                nameof(options)
            );
        }

        return await this._quizQuestionRepository.AddQuestionAsync(quizQuestion, options);
    }

    public async Task<bool> UpdateQuestionAsync(
        Guid quizQuestionId,
        QuizQuestionViewModel quizQuestion,
        List<QuestionOptionViewModel> options
    )
    {
        var existingQuestion =
            await this._quizQuestionRepository.GetQuestionByIdAsync(quizQuestionId)
            ?? throw new ArgumentException("Question not found.", nameof(quizQuestionId));

        if (
            !quizQuestion.QuestionType.Equals(
                existingQuestion.QuestionType,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            throw new InvalidOperationException("Question type cannot be updated.");
        }

        if (!ValidateOptions(existingQuestion.QuestionType, options))
        {
            throw new ArgumentException(
                "Invalid options for the given question type.",
                nameof(options)
            );
        }

        return await this._quizQuestionRepository.UpdateQuestionAsync(
            quizQuestionId,
            quizQuestion,
            options
        );
    }

    public async Task<bool> DeleteQuestionAsync(Guid quizQuestionId)
    {
        var existingQuestion = await this._quizQuestionRepository.GetQuestionByIdAsync(
            quizQuestionId
        );
        if (existingQuestion == null)
        {
            return false;
        }

        var deleted = await this._quizQuestionRepository.DeleteQuestionAsync(quizQuestionId);
        if (deleted)
        {
            this._quizQuestionRepository.ReorderQuestionNos(
                existingQuestion.QuizId,
                existingQuestion.QuestionNo
            );
        }
        return deleted;
    }

    public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync() =>
        await this._quizQuestionRepository.GetAllQuestionsAsync();

    public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId) =>
        await this._quizQuestionRepository.GetAllQuestionsByQuizIdAsync(quizId);

    public async Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId) =>
        await this._quizQuestionRepository.GetQuestionByIdAsync(quizQuestionId);

    private static bool IsValidQuestionType(string questionType) =>
        questionType
            is QuizQuestionTypes.MultiSelectQuestion
                or QuizQuestionTypes.MultiChoiceQuestion
                or QuizQuestionTypes.TrueFalseQuestion;

    private static bool ValidateOptions(string questionType, List<QuestionOptionViewModel> options)
    {
        if (questionType == QuizQuestionTypes.TrueFalseQuestion)
        {
            return ValidateTrueFalseOptions(options);
        }
        else
        {
            return ValidateUniqueOptions(options)
                && ValidateOptionsByQuestionType(questionType, options);
        }
    }

    private static bool ValidateTrueFalseOptions(List<QuestionOptionViewModel> options)
    {
        if (options.Count != 2)
        {
            return false;
        }

        var trueOption = options.Any(o =>
            o.Option.Equals("true", StringComparison.OrdinalIgnoreCase)
        );
        var falseOption = options.Any(o =>
            o.Option.Equals("false", StringComparison.OrdinalIgnoreCase)
        );

        return trueOption && falseOption;
    }

    private static bool ValidateUniqueOptions(List<QuestionOptionViewModel> options)
    {
        var distinctOptions = options
            .Select(o => o.Option.ToLower(System.Globalization.CultureInfo.CurrentCulture))
            .Distinct()
            .Count();
        return distinctOptions == options.Count;
    }

    private static bool ValidateOptionsByQuestionType(
        string questionType,
        List<QuestionOptionViewModel> options
    ) =>
        questionType switch
        {
            QuizQuestionTypes.MultiSelectQuestion
                => options.Count >= 5
                    && options.Count <= 8
                    && options.Count(o => o.IsCorrect) >= 2
                    && options.Count(o => o.IsCorrect) <= 3,
            QuizQuestionTypes.MultiChoiceQuestion
                => options.Count == 4 && options.Count(o => o.IsCorrect) == 1,
            QuizQuestionTypes.TrueFalseQuestion
                => options.Count == 2 && options.Count(o => o.IsCorrect) == 1,
            _ => false,
        };
}

//using LXP.Common.ViewModels.QuizQuestionViewModel;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;

//namespace LXP.Core.Services

//            _quizQuestionRepository = quizQuestionRepository;

//            QuizQuestionViewModel quizQuestion,
//            List<QuestionOptionViewModel> options
//        )

//            Guid quizQuestionId,
//            QuizQuestionViewModel quizQuestion,
//            List<QuestionOptionViewModel> options
//        )
//                quizQuestionId,
//                quizQuestion,
//                options
//            );


