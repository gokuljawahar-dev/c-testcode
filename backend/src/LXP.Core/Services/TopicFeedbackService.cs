namespace LXP.Core.Services;

using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class TopicFeedbackService(ITopicFeedbackRepository topicFeedbackRepository)
    : ITopicFeedbackService
{
    private readonly ITopicFeedbackRepository _topicFeedbackRepository = topicFeedbackRepository;

    public Guid AddFeedbackQuestion(
        TopicFeedbackQuestionViewModel topicFeedbackQuestion,
        List<TopicFeedbackQuestionsOptionViewModel> options
    )
    {
        var normalizedQuestionType = topicFeedbackQuestion.QuestionType.ToUpper(
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

        if (!ValidateOptionsByFeedbackQuestionType(topicFeedbackQuestion.QuestionType, options))
        {
            throw new ArgumentException(
                "Invalid options for the given question type.",
                nameof(options)
            );
        }

        var questionEntity = new TopicFeedbackQuestion
        {
            TopicId = topicFeedbackQuestion.TopicId,
            QuestionNo = this._topicFeedbackRepository.GetNextFeedbackQuestionNo(
                topicFeedbackQuestion.TopicId
            ),
            Question = topicFeedbackQuestion.Question,
            QuestionType = normalizedQuestionType,
            CreatedBy = "Admin",
            CreatedAt = DateTime.Now
        };

        this._topicFeedbackRepository.AddFeedbackQuestion(questionEntity);

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
                        TopicFeedbackQuestionId = questionEntity.TopicFeedbackQuestionId,
                        OptionText = option.OptionText,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "Admin"
                    })
                    .ToList();

                this._topicFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
            }
        }

        return questionEntity.TopicFeedbackQuestionId;
    }

    public List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions() =>
        this._topicFeedbackRepository.GetAllFeedbackQuestions();

    public TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid topicFeedbackQuestionId) =>
        this._topicFeedbackRepository.GetFeedbackQuestionById(topicFeedbackQuestionId);

    public bool UpdateFeedbackQuestion(
        Guid topicFeedbackQuestionId,
        TopicFeedbackQuestionViewModel topicFeedbackQuestion,
        List<TopicFeedbackQuestionsOptionViewModel> options
    )
    {
        var existingQuestion = this._topicFeedbackRepository.GetTopicFeedbackQuestionEntityById(
            topicFeedbackQuestionId
        );
        if (existingQuestion != null)
        {
            if (
                !existingQuestion.QuestionType.Equals(
                    topicFeedbackQuestion.QuestionType,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                throw new InvalidOperationException("Question type cannot be modified.");
            }

            existingQuestion.Question = topicFeedbackQuestion.Question;
            existingQuestion.ModifiedAt = DateTime.Now;
            existingQuestion.ModifiedBy = "Admin";
            this._topicFeedbackRepository.UpdateFeedbackQuestion(existingQuestion);

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

                var existingOptions = this._topicFeedbackRepository.GetFeedbackQuestionOptionsById(
                    topicFeedbackQuestionId
                );
                this._topicFeedbackRepository.RemoveFeedbackQuestionOptions(existingOptions);

                if (options != null && options.Count > 0)
                {
                    var optionEntities = options
                        .Select(option => new FeedbackQuestionsOption
                        {
                            TopicFeedbackQuestionId = topicFeedbackQuestionId,
                            OptionText = option.OptionText,
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin"
                        })
                        .ToList();

                    this._topicFeedbackRepository.AddFeedbackQuestionOptions(optionEntities);
                }
            }

            return true;
        }
        return false;
    }

    public bool DeleteFeedbackQuestion(Guid topicFeedbackQuestionId)
    {
        try
        {
            var existingQuestion = this._topicFeedbackRepository.GetTopicFeedbackQuestionEntityById(
                topicFeedbackQuestionId
            );
            if (existingQuestion != null)
            {
                var relatedOptions = this._topicFeedbackRepository.GetFeedbackQuestionOptionsById(
                    topicFeedbackQuestionId
                );

                if (relatedOptions.Count != 0)
                {
                    this._topicFeedbackRepository.RemoveFeedbackQuestionOptions(relatedOptions);
                }

                // Remove the FeedbackQuestion and related FeedbackResponses
                this._topicFeedbackRepository.RemoveFeedbackQuestion(existingQuestion);
                this._topicFeedbackRepository.ReorderQuestionNos(
                    existingQuestion.TopicId,
                    existingQuestion.QuestionNo
                );

                return true;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while deleting the feedback question.",
                ex
            );
        }
        return false;
    }

    public List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId) =>
        this._topicFeedbackRepository.GetFeedbackQuestionsByTopicId(topicId);

    public bool DeleteFeedbackQuestionsByTopicId(Guid topicId)
    {
        try
        {
            var questions = this._topicFeedbackRepository.GetFeedbackQuestionsByTopicId(topicId);
            if (questions == null || questions.Count == 0)
            {
                return false;
            }

            foreach (var question in questions)
            {
                this.DeleteFeedbackQuestion(question.TopicFeedbackQuestionId);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static bool ValidateOptionsByFeedbackQuestionType(
        string questionType,
        List<TopicFeedbackQuestionsOptionViewModel> options
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
}
