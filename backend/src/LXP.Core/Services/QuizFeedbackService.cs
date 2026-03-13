namespace LXP.Core.Services;

using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class QuizFeedbackService(IQuizFeedbackRepository quizFeedbackRepository)
    : IQuizFeedbackService
{
    private readonly IQuizFeedbackRepository _quizFeedbackRepository = quizFeedbackRepository;

    public Guid AddFeedbackQuestion(
        QuizfeedbackquestionViewModel quizfeedbackquestion,
        List<QuizFeedbackQuestionsOptionViewModel> options
    )
    {
        // Normalize question type to uppercase
        var normalizedQuestionType = quizfeedbackquestion.QuestionType.ToUpper(
            System.Globalization.CultureInfo.CurrentCulture
        );

        // Ensure no options are saved for descriptive questions
        if (
            normalizedQuestionType.Equals(
                FeedbackQuestionTypes.DescriptiveQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            options = null;
        }

        if (!ValidateOptionsByFeedbackQuestionType(quizfeedbackquestion.QuestionType, options))
        {
            throw new ArgumentException(
                "Invalid options for the given question type.",
                nameof(options)
            );
        }

        // Create the feedback question entity
        var questionEntity = new QuizFeedbackQuestion
        {
            QuizId = quizfeedbackquestion.QuizId,
            QuestionNo = this._quizFeedbackRepository.GetNextFeedbackQuestionNo(
                quizfeedbackquestion.QuizId
            ),
            Question = quizfeedbackquestion.Question,
            QuestionType = normalizedQuestionType,
            CreatedBy = "Admin",
            CreatedAt = DateTime.Now
        };

        this._quizFeedbackRepository.AddFeedbackQuestion(questionEntity);

        // Save the options only if the question type is MCQ
        if (
            normalizedQuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
            && options != null
            && options.Count > 0
        )
        {
            var optionEntities = options
                .Select(option => new FeedbackQuestionsOption
                {
                    QuizFeedbackQuestionId = questionEntity.QuizFeedbackQuestionId,
                    OptionText = option.OptionText,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                })
                .ToList();

            this._quizFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
        }

        return questionEntity.QuizFeedbackQuestionId;
    }

    public List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions() =>
        this._quizFeedbackRepository.GetAllFeedbackQuestions();

    public QuizfeedbackquestionNoViewModel GetFeedbackQuestionById(Guid QuizFeedbackQuestionId)
    {
        var feedbackQuestion = this._quizFeedbackRepository.GetFeedbackQuestionEntityById(
            QuizFeedbackQuestionId
        );

        if (feedbackQuestion == null)
        {
            return null;
        }

        var options = this._quizFeedbackRepository.GetFeedbackQuestionOptions(
            feedbackQuestion.QuizFeedbackQuestionId
        );

        return new QuizfeedbackquestionNoViewModel
        {
            QuizFeedbackQuestionId = feedbackQuestion.QuizFeedbackQuestionId,
            QuizId = feedbackQuestion.QuizId,
            QuestionNo = feedbackQuestion.QuestionNo,
            Question = feedbackQuestion.Question,
            QuestionType = feedbackQuestion.QuestionType,
            Options = options
                .Select(o => new QuizFeedbackQuestionsOptionViewModel { OptionText = o.OptionText })
                .ToList()
        };
    }

    public bool UpdateFeedbackQuestion(
        Guid quizFeedbackQuestionId,
        QuizfeedbackquestionViewModel quizfeedbackquestion,
        List<QuizFeedbackQuestionsOptionViewModel> options
    )
    {
        var existingQuestion = this._quizFeedbackRepository.GetFeedbackQuestionEntityById(
            quizFeedbackQuestionId
        );

        if (existingQuestion == null)
        {
            return false;
        }

        // Check if the question type is being modified
        if (
            !existingQuestion.QuestionType.Equals(
                quizfeedbackquestion.QuestionType,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            throw new InvalidOperationException("Question type cannot be modified.");
        }

        // Update the question details
        existingQuestion.Question = quizfeedbackquestion.Question;
        existingQuestion.ModifiedAt = DateTime.Now;
        existingQuestion.ModifiedBy = "Admin";
        this._quizFeedbackRepository.UpdateFeedbackQuestion(existingQuestion);

        // Handle options only if the question type is MCQ
        if (
            existingQuestion.QuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            if (!ValidateOptionsByFeedbackQuestionType(existingQuestion.QuestionType, options))
            {
                throw new ArgumentException("Invalid options for the given question type.");
            }

            var existingOptions = this._quizFeedbackRepository.GetFeedbackQuestionOptions(
                quizFeedbackQuestionId
            );
            this._quizFeedbackRepository.DeleteFeedbackQuestionOptions(existingOptions);

            if (options != null && options.Count > 0)
            {
                var optionEntities = options
                    .Select(option => new FeedbackQuestionsOption
                    {
                        QuizFeedbackQuestionId = quizFeedbackQuestionId,
                        OptionText = option.OptionText,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "Admin"
                    })
                    .ToList();

                this._quizFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
            }
        }

        return true;
    }

    public bool DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
    {
        var existingQuestion = this._quizFeedbackRepository.GetFeedbackQuestionEntityById(
            quizFeedbackQuestionId
        );

        if (existingQuestion == null)
        {
            return false;
        }

        var relatedResponses = this._quizFeedbackRepository.GetFeedbackResponsesByQuestionId(
            quizFeedbackQuestionId
        );
        if (relatedResponses.Count != 0)
        {
            this._quizFeedbackRepository.DeleteFeedbackResponses(relatedResponses);
        }

        var relatedOptions = this._quizFeedbackRepository.GetFeedbackQuestionOptions(
            quizFeedbackQuestionId
        );
        if (relatedOptions.Count != 0)
        {
            this._quizFeedbackRepository.DeleteFeedbackQuestionOptions(relatedOptions);
        }

        this._quizFeedbackRepository.DeleteFeedbackQuestion(existingQuestion);

        this.ReorderQuestionNos(existingQuestion.QuizId, existingQuestion.QuestionNo);

        return true;
    }

    public List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId) =>
        this._quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId);

    public bool DeleteFeedbackQuestionsByQuizId(Guid quizId)
    {
        var questions = this._quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId);
        if (questions == null || questions.Count == 0)
        {
            return false;
        }

        foreach (var question in questions)
        {
            this.DeleteFeedbackQuestion(question.QuizFeedbackQuestionId);
        }

        return true;
    }

    private static bool ValidateOptionsByFeedbackQuestionType(
        string questionType,
        List<QuizFeedbackQuestionsOptionViewModel> options
    )
    {
        questionType = questionType.ToUpper(System.Globalization.CultureInfo.CurrentCulture);

        if (
            questionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            return options != null && options.Count >= 2 && options.Count <= 5;
        }
        return options == null || options.Count == 0;
    }

    private void ReorderQuestionNos(Guid quizId, int deletedQuestionNo)
    {
        var questionsToUpdate = this
            ._quizFeedbackRepository.GetFeedbackQuestionsByQuizId(quizId)
            .Where(q => q.QuestionNo > deletedQuestionNo)
            .OrderBy(q => q.QuestionNo)
            .ToList();

        foreach (var question in questionsToUpdate)
        {
            var questionEntity = this._quizFeedbackRepository.GetFeedbackQuestionEntityById(
                question.QuizFeedbackQuestionId
            );
            if (questionEntity != null)
            {
                questionEntity.QuestionNo--;
                this._quizFeedbackRepository.UpdateFeedbackQuestion(questionEntity);
            }
        }
    }
}
