namespace LXP.Core.Services;

using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class FeedbackResponseDetailsService(
    IFeedbackResponseDetailsRepository feedbackResponseDetailsRepository
) : IFeedbackResponseDetailsService
{
    private readonly IFeedbackResponseDetailsRepository _feedbackResponseDetailsRepository =
        feedbackResponseDetailsRepository;

    public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponses(Guid quizId) =>
        this._feedbackResponseDetailsRepository.GetQuizFeedbackResponses(quizId);

    public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponses(Guid topicId) =>
        this._feedbackResponseDetailsRepository.GetTopicFeedbackResponses(topicId);

    public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponsesByLearner(
        Guid quizId,
        Guid learnerId
    ) =>
        this._feedbackResponseDetailsRepository.GetQuizFeedbackResponsesByLearner(
            quizId,
            learnerId
        );

    public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponsesByLearner(
        Guid topicId,
        Guid learnerId
    ) =>
        this._feedbackResponseDetailsRepository.GetTopicFeedbackResponsesByLearner(
            topicId,
            learnerId
        );

    public List<QuizFeedbackResponseDetailsViewModel> GetAllQuizFeedbackResponses() =>
        this._feedbackResponseDetailsRepository.GetAllQuizFeedbackResponses();

    public List<TopicFeedbackResponseDetailsViewModel> GetAllTopicFeedbackResponses() =>
        this._feedbackResponseDetailsRepository.GetAllTopicFeedbackResponses();

    public List<CourseFeedbackResponseDetailsViewModel> GetCourseFeedbackResponses(Guid courseId) =>
        this._feedbackResponseDetailsRepository.GetCourseFeedbackResponses(courseId);

    public List<CourseFeedbackResponseDetailsViewModel> GetCourseFeedbackResponsesByLearner(
        Guid courseId,
        Guid learnerId
    ) =>
        this._feedbackResponseDetailsRepository.GetCourseFeedbackResponsesByLearner(
            courseId,
            learnerId
        );

    public List<CourseFeedbackResponseDetailsViewModel> GetAllCourseFeedbackResponses() =>
        this._feedbackResponseDetailsRepository.GetAllCourseFeedbackResponses();
}
