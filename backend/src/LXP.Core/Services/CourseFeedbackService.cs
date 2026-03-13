namespace LXP.Core.Services;

using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class CourseFeedbackService(ICourseFeedbackRepository courseFeedbackRepository)
    : ICourseFeedbackService
{
    private readonly ICourseFeedbackRepository _courseFeedbackRepository = courseFeedbackRepository;

    public Guid AddFeedbackQuestion(
        CourseFeedbackQuestionViewModel courseFeedbackQuestion,
        List<CourseFeedbackQuestionsOptionViewModel> options
    )
    {
        var normalizedQuestionType = courseFeedbackQuestion.QuestionType.ToUpper(
            System.Globalization.CultureInfo.CurrentCulture
        );

        if (
            normalizedQuestionType.Equals(
                FeedbackQuestionTypes.DescriptiveQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            options = null;
        }

        if (!ValidateOptionsByFeedbackQuestionType(courseFeedbackQuestion.QuestionType, options))
        {
            throw new ArgumentException(
                "Invalid options for the given question type.",
                nameof(options)
            );
        }

        var questionEntity = new CourseFeedbackQuestion
        {
            CourseId = courseFeedbackQuestion.CourseId,
            QuestionNo = this._courseFeedbackRepository.GetNextFeedbackQuestionNo(
                courseFeedbackQuestion.CourseId
            ),
            Question = courseFeedbackQuestion.Question,
            QuestionType = normalizedQuestionType,
            CreatedBy = "Admin",
            CreatedAt = DateTime.Now
        };

        this._courseFeedbackRepository.AddFeedbackQuestion(questionEntity);

        if (
            normalizedQuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            if (options != null && options.Count > 0)
            {
                var optionEntities = options
                    .Select(option => new FeedbackQuestionsOption
                    {
                        CourseFeedbackQuestionId = questionEntity.CourseFeedbackQuestionId,
                        OptionText = option.OptionText,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "Admin"
                    })
                    .ToList();

                this._courseFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
            }
        }

        return questionEntity.CourseFeedbackQuestionId;
    }

    public List<CourseFeedbackQuestionNoViewModel> GetAllFeedbackQuestions() =>
        this._courseFeedbackRepository.GetAllFeedbackQuestions();

    public CourseFeedbackQuestionNoViewModel GetFeedbackQuestionById(
        Guid courseFeedbackQuestionId
    ) => this._courseFeedbackRepository.GetFeedbackQuestionById(courseFeedbackQuestionId);

    public bool UpdateFeedbackQuestion(
        Guid courseFeedbackQuestionId,
        CourseFeedbackQuestionViewModel courseFeedbackQuestion,
        List<CourseFeedbackQuestionsOptionViewModel> options
    )
    {
        var existingQuestion = this._courseFeedbackRepository.GetCourseFeedbackQuestionEntityById(
            courseFeedbackQuestionId
        );
        if (existingQuestion != null)
        {
            if (
                !existingQuestion.QuestionType.Equals(
                    courseFeedbackQuestion.QuestionType,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                throw new InvalidOperationException("Question type cannot be modified.");
            }

            existingQuestion.Question = courseFeedbackQuestion.Question;
            existingQuestion.ModifiedAt = DateTime.Now;
            existingQuestion.ModifiedBy = "Admin";
            this._courseFeedbackRepository.UpdateFeedbackQuestion(existingQuestion);

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

                var existingOptions = this._courseFeedbackRepository.GetFeedbackQuestionOptionsById(
                    courseFeedbackQuestionId
                );
                this._courseFeedbackRepository.RemoveFeedbackQuestionOptions(existingOptions);

                if (options != null && options.Count > 0)
                {
                    var optionEntities = options
                        .Select(option => new FeedbackQuestionsOption
                        {
                            CourseFeedbackQuestionId = courseFeedbackQuestionId,
                            OptionText = option.OptionText,
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin"
                        })
                        .ToList();

                    this._courseFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
                }
            }

            return true;
        }
        return false;
    }

    public bool DeleteFeedbackQuestion(Guid courseFeedbackQuestionId)
    {
        var existingQuestion = this._courseFeedbackRepository.GetCourseFeedbackQuestionEntityById(
            courseFeedbackQuestionId
        );
        if (existingQuestion != null)
        {
            var options = this._courseFeedbackRepository.GetFeedbackQuestionOptionsById(
                courseFeedbackQuestionId
            );
            this._courseFeedbackRepository.RemoveFeedbackQuestionOptions(options);
            this._courseFeedbackRepository.DeleteFeedbackQuestion(existingQuestion);
            return true;
        }
        return false;
    }

    public List<CourseFeedbackQuestionNoViewModel> GetFeedbackQuestionsByCourseId(Guid courseId) =>
        this._courseFeedbackRepository.GetFeedbackQuestionsByCourseId(courseId);

    private static bool ValidateOptionsByFeedbackQuestionType(
        string questionType,
        List<CourseFeedbackQuestionsOptionViewModel> options
    )
    {
        if (string.IsNullOrWhiteSpace(questionType))
        {
            return false;
        }

        var normalizedQuestionType = questionType.ToUpper(
            System.Globalization.CultureInfo.CurrentCulture
        );
        if (
            normalizedQuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            return options != null && options.Count > 0;
        }

        return true;
    }
}
