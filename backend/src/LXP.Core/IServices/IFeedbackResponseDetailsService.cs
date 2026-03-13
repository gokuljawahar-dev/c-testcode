namespace LXP.Core.IServices;

using LXP.Common.ViewModels.FeedbackResponseViewModel;

public interface IFeedbackResponseDetailsService
{
    List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponses(Guid quizId);
    List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponses(Guid topicId);
    List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponsesByLearner(
        Guid quizId,
        Guid learnerId
    );
    List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponsesByLearner(
        Guid topicId,
        Guid learnerId
    );
    List<QuizFeedbackResponseDetailsViewModel> GetAllQuizFeedbackResponses();
    List<TopicFeedbackResponseDetailsViewModel> GetAllTopicFeedbackResponses();

    List<CourseFeedbackResponseDetailsViewModel> GetCourseFeedbackResponses(Guid courseId);
    List<CourseFeedbackResponseDetailsViewModel> GetCourseFeedbackResponsesByLearner(
        Guid courseId,
        Guid learnerId
    );
    List<CourseFeedbackResponseDetailsViewModel> GetAllCourseFeedbackResponses();
}
