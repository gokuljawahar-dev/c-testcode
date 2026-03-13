namespace LXP.Core.Services;

using FluentValidation;
using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.Validators;
using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class FeedbackResponseService(IFeedbackResponseRepository feedbackResponseRepository)
    : IFeedbackResponseService
{
    private readonly IFeedbackResponseRepository _feedbackResponseRepository =
        feedbackResponseRepository;
    private readonly QuizFeedbackResponseViewModelValidator _quizFeedbackValidator = new();
    private readonly TopicFeedbackResponseViewModelValidator _topicFeedbackValidator = new();
    private readonly CourseFeedbackResponseViewModelValidator _courseFeedbackValidator = new();

    public void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse) =>
        this.ValidateAndSubmitQuizFeedback(feedbackResponse);

    public void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse) =>
        this.ValidateAndSubmitTopicFeedback(feedbackResponse);

    public void SubmitFeedbackResponses(
        IEnumerable<QuizFeedbackResponseViewModel> feedbackResponses
    )
    {
        foreach (var feedbackResponse in feedbackResponses)
        {
            this.ValidateAndSubmitQuizFeedback(feedbackResponse);
        }
    }

    public void SubmitFeedbackResponses(
        IEnumerable<TopicFeedbackResponseViewModel> feedbackResponses
    )
    {
        foreach (var feedbackResponse in feedbackResponses)
        {
            this.ValidateAndSubmitTopicFeedback(feedbackResponse);
        }
    }

    private void ValidateAndSubmitQuizFeedback(QuizFeedbackResponseViewModel feedbackResponse)
    {
        var validationResult = this._quizFeedbackValidator.Validate(feedbackResponse);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        ArgumentNullException.ThrowIfNull(feedbackResponse);

        var question =
            this._feedbackResponseRepository.GetQuizFeedbackQuestion(
                feedbackResponse.QuizFeedbackQuestionId
            )
            ?? throw new ArgumentException(
                "Invalid feedback question ID.",
                nameof(feedbackResponse.QuizFeedbackQuestionId)
            );

        var learner =
            this._feedbackResponseRepository.GetLearner(feedbackResponse.LearnerId)
            ?? throw new ArgumentException(
                "Invalid learner ID.",
                nameof(feedbackResponse.LearnerId)
            );

        var existingResponse = this._feedbackResponseRepository.GetExistingQuizFeedbackResponse(
            feedbackResponse.QuizFeedbackQuestionId,
            feedbackResponse.LearnerId
        );

        if (existingResponse != null)
        {
            throw new InvalidOperationException(
                "User has already submitted a response for this question."
            );
        }

        Guid? optionId = null;

        if (
            question.QuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            if (string.IsNullOrEmpty(feedbackResponse.OptionText))
            {
                throw new ArgumentException("Option text must be provided for MCQ responses.");
            }

            optionId = this._feedbackResponseRepository.GetOptionIdByText(
                feedbackResponse.QuizFeedbackQuestionId,
                feedbackResponse.OptionText
            );
            if (optionId == null)
            {
                throw new ArgumentException(
                    "Invalid option text provided.",
                    nameof(feedbackResponse.OptionText)
                );
            }

            feedbackResponse.Response = null;
        }

        var response = new FeedbackResponse
        {
            QuizFeedbackQuestionId = feedbackResponse.QuizFeedbackQuestionId,
            LearnerId = feedbackResponse.LearnerId,
            Response = feedbackResponse.Response,
            OptionId = optionId,
            GeneratedAt = DateTime.Now,
            GeneratedBy = "learner"
        };

        this._feedbackResponseRepository.AddFeedbackResponse(response);
        feedbackResponse.QuizId = question.QuizId;
    }

    private void ValidateAndSubmitTopicFeedback(TopicFeedbackResponseViewModel feedbackResponse)
    {
        var validationResult = this._topicFeedbackValidator.Validate(feedbackResponse);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        ArgumentNullException.ThrowIfNull(feedbackResponse);

        var question =
            this._feedbackResponseRepository.GetTopicFeedbackQuestion(
                feedbackResponse.TopicFeedbackQuestionId
            )
            ?? throw new ArgumentException(
                "Invalid feedback question ID.",
                nameof(feedbackResponse.TopicFeedbackQuestionId)
            );

        var learner =
            this._feedbackResponseRepository.GetLearner(feedbackResponse.LearnerId)
            ?? throw new ArgumentException(
                "Invalid learner ID.",
                nameof(feedbackResponse.LearnerId)
            );

        var existingResponse = this._feedbackResponseRepository.GetExistingTopicFeedbackResponse(
            feedbackResponse.TopicFeedbackQuestionId,
            feedbackResponse.LearnerId
        );

        if (existingResponse != null)
        {
            throw new InvalidOperationException(
                "User has already submitted a response for this question."
            );
        }

        Guid? optionId = null;

        if (
            question.QuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            if (string.IsNullOrEmpty(feedbackResponse.OptionText))
            {
                throw new ArgumentException("Option text must be provided for MCQ responses.");
            }

            optionId = this._feedbackResponseRepository.GetOptionIdByText(
                feedbackResponse.TopicFeedbackQuestionId,
                feedbackResponse.OptionText
            );
            if (optionId == null)
            {
                throw new ArgumentException(
                    "Invalid option text provided.",
                    nameof(feedbackResponse.OptionText)
                );
            }

            feedbackResponse.Response = null;
        }

        var response = new FeedbackResponse
        {
            TopicFeedbackQuestionId = feedbackResponse.TopicFeedbackQuestionId,
            LearnerId = feedbackResponse.LearnerId,
            Response = feedbackResponse.Response,
            OptionId = optionId,
            GeneratedAt = DateTime.Now,
            GeneratedBy = "learner"
        };

        this._feedbackResponseRepository.AddFeedbackResponse(response);
        feedbackResponse.TopicId = question.TopicId;
    }

    public LearnerFeedbackStatusViewModel GetQuizFeedbackStatus(Guid learnerId, Guid quizId)
    {
        var allQuestions = this._feedbackResponseRepository.GetQuizFeedbackQuestions(quizId);
        var submittedResponses = this._feedbackResponseRepository.GetQuizFeedbackResponsesByLearner(
            learnerId,
            quizId
        );

        var isQuizFeedbackSubmitted =
            allQuestions.Any() && allQuestions.Count() == submittedResponses.Count();

        return new LearnerFeedbackStatusViewModel
        {
            LearnerId = learnerId,
            IsQuizFeedbackSubmitted = isQuizFeedbackSubmitted
        };
    }

    public LearnerFeedbackStatusViewModel GetTopicFeedbackStatus(Guid learnerId, Guid topicId)
    {
        var allQuestions = this._feedbackResponseRepository.GetTopicFeedbackQuestions(topicId);
        var submittedResponses =
            this._feedbackResponseRepository.GetTopicFeedbackResponsesByLearner(learnerId, topicId);

        var isTopicFeedbackSubmitted =
            allQuestions.Any() && allQuestions.Count() == submittedResponses.Count();

        return new LearnerFeedbackStatusViewModel
        {
            LearnerId = learnerId,
            IsTopicFeedbackSubmitted = isTopicFeedbackSubmitted
        };
    }

    public void SubmitFeedbackResponse(CourseFeedbackResponseViewModel feedbackResponse) =>
        this.ValidateAndSubmitCourseFeedback(feedbackResponse);

    public void SubmitFeedbackResponses(
        IEnumerable<CourseFeedbackResponseViewModel> feedbackResponses
    )
    {
        foreach (var feedbackResponse in feedbackResponses)
        {
            this.ValidateAndSubmitCourseFeedback(feedbackResponse);
        }
    }

    private void ValidateAndSubmitCourseFeedback(CourseFeedbackResponseViewModel feedbackResponse)
    {
        var validationResult = this._courseFeedbackValidator.Validate(feedbackResponse);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        ArgumentNullException.ThrowIfNull(feedbackResponse);

        var question =
            this._feedbackResponseRepository.GetCourseFeedbackQuestion(
                feedbackResponse.CourseFeedbackQuestionId
            )
            ?? throw new ArgumentException(
                "Invalid feedback question ID.",
                nameof(feedbackResponse.CourseFeedbackQuestionId)
            );

        var learner =
            this._feedbackResponseRepository.GetLearner(feedbackResponse.LearnerId)
            ?? throw new ArgumentException(
                "Invalid learner ID.",
                nameof(feedbackResponse.LearnerId)
            );

        var existingResponse = this._feedbackResponseRepository.GetExistingCourseFeedbackResponse(
            feedbackResponse.CourseFeedbackQuestionId,
            feedbackResponse.LearnerId
        );

        if (existingResponse != null)
        {
            throw new InvalidOperationException(
                "User has already submitted a response for this question."
            );
        }

        Guid? optionId = null;

        if (
            question.QuestionType.Equals(
                FeedbackQuestionTypes.MultiChoiceQuestion,
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            if (string.IsNullOrEmpty(feedbackResponse.OptionText))
            {
                throw new ArgumentException("Option text must be provided for MCQ responses.");
            }

            optionId = this._feedbackResponseRepository.GetOptionIdByText(
                feedbackResponse.CourseFeedbackQuestionId,
                feedbackResponse.OptionText
            );
            if (optionId == null)
            {
                throw new ArgumentException(
                    "Invalid option text provided.",
                    nameof(feedbackResponse.OptionText)
                );
            }

            feedbackResponse.Response = null;
        }

        var response = new FeedbackResponse
        {
            CourseFeedbackQuestionId = feedbackResponse.CourseFeedbackQuestionId,
            LearnerId = feedbackResponse.LearnerId,
            Response = feedbackResponse.Response,
            OptionId = optionId,
            GeneratedAt = DateTime.Now,
            GeneratedBy = "learner"
        };

        this._feedbackResponseRepository.AddFeedbackResponse(response);
        feedbackResponse.CourseId = question.CourseId;
    }

    public LearnerFeedbackStatusViewModel GetCourseFeedbackStatus(Guid learnerId, Guid courseId)
    {
        var allQuestions = this._feedbackResponseRepository.GetCourseFeedbackQuestions(courseId);
        var submittedResponses =
            this._feedbackResponseRepository.GetCourseFeedbackResponsesByLearner(
                learnerId,
                courseId
            );

        var isCourseFeedbackSubmitted =
            allQuestions.Any() && allQuestions.Count() == submittedResponses.Count();

        return new LearnerFeedbackStatusViewModel
        {
            LearnerId = learnerId,
            IsCourseFeedbackSubmitted = isCourseFeedbackSubmitted
        };
    }
}
