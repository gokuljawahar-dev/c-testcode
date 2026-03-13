namespace LXP.Core.IServices;

using LXP.Common.ViewModels.FeedbackResponseViewModel;

public interface IFeedbackResponseService
{
    void SubmitFeedbackResponse(QuizFeedbackResponseViewModel feedbackResponse);
    void SubmitFeedbackResponse(TopicFeedbackResponseViewModel feedbackResponse);
    void SubmitFeedbackResponses(IEnumerable<QuizFeedbackResponseViewModel> feedbackResponses);
    void SubmitFeedbackResponses(IEnumerable<TopicFeedbackResponseViewModel> feedbackResponses);

    //new
    LearnerFeedbackStatusViewModel GetQuizFeedbackStatus(Guid learnerId, Guid quizId);
    LearnerFeedbackStatusViewModel GetTopicFeedbackStatus(Guid learnerId, Guid topicId);

    void SubmitFeedbackResponse(CourseFeedbackResponseViewModel feedbackResponse);
    void SubmitFeedbackResponses(IEnumerable<CourseFeedbackResponseViewModel> feedbackResponses);
    LearnerFeedbackStatusViewModel GetCourseFeedbackStatus(Guid learnerId, Guid courseId);
}
