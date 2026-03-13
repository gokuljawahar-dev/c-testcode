namespace LXP.Data.IRepository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViewModel;

public interface ITopicFeedbackRepository
{
    void AddFeedbackQuestion(TopicFeedbackQuestion questionEntity);
    void AddFeedbackQuestionOptions(List<FeedbackQuestionsOption> options);
    List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
    List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId);
    int GetNextFeedbackQuestionNo(Guid topicId);
    TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid topicFeedbackQuestionId);
    void UpdateFeedbackQuestion(TopicFeedbackQuestion questionEntity);
    void RemoveFeedbackQuestion(TopicFeedbackQuestion questionEntity);
    void RemoveFeedbackQuestionOptions(List<FeedbackQuestionsOption> options);
    void ReorderQuestionNos(Guid topicId, int deletedQuestionNo);
    List<FeedbackQuestionsOption> GetFeedbackQuestionOptionsById(Guid topicFeedbackQuestionId);
    TopicFeedbackQuestion GetTopicFeedbackQuestionEntityById(Guid topicFeedbackQuestionId);
}
