namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IFeedbackResponseRepository
{
    QuizFeedbackQuestion GetQuizFeedbackQuestion(Guid quizFeedbackQuestionId);
    TopicFeedbackQuestion GetTopicFeedbackQuestion(Guid topicFeedbackQuestionId);
    Learner GetLearner(Guid learnerId);
    FeedbackResponse GetExistingQuizFeedbackResponse(Guid quizFeedbackQuestionId, Guid learnerId);
    FeedbackResponse GetExistingTopicFeedbackResponse(Guid topicFeedbackQuestionId, Guid learnerId);
    void AddFeedbackResponse(FeedbackResponse feedbackResponse);
    Guid? GetOptionIdByText(Guid questionId, string optionText);

    //new bug fix
    void DeleteFeedbackResponsesByQuizQuestionId(Guid quizFeedbackQuestionId);
    void DeleteFeedbackResponsesByTopicQuestionId(Guid topicFeedbackQuestionId);

    IEnumerable<QuizFeedbackQuestion> GetQuizFeedbackQuestions(Guid quizId);
    IEnumerable<FeedbackResponse> GetQuizFeedbackResponsesByLearner(Guid learnerId, Guid quizId);
    IEnumerable<TopicFeedbackQuestion> GetTopicFeedbackQuestions(Guid topicId);
    IEnumerable<FeedbackResponse> GetTopicFeedbackResponsesByLearner(Guid learnerId, Guid topicId);

    CourseFeedbackQuestion GetCourseFeedbackQuestion(Guid courseFeedbackQuestionId);
    FeedbackResponse GetExistingCourseFeedbackResponse(
        Guid courseFeedbackQuestionId,
        Guid learnerId
    );
    IEnumerable<CourseFeedbackQuestion> GetCourseFeedbackQuestions(Guid courseId);
    IEnumerable<FeedbackResponse> GetCourseFeedbackResponsesByLearner(
        Guid learnerId,
        Guid courseId
    );
}
