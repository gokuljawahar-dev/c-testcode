namespace LXP.Data.IRepository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;

public interface ICourseFeedbackRepository
{
    void AddFeedbackQuestion(CourseFeedbackQuestion coursefeedbackquestion);
    void AddFeedbackQuestionOptions(List<FeedbackQuestionsOption> options);
    List<CourseFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
    CourseFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid courseFeedbackQuestionId);
    CourseFeedbackQuestion GetCourseFeedbackQuestionEntityById(Guid courseFeedbackQuestionId);
    void UpdateFeedbackQuestion(CourseFeedbackQuestion coursefeedbackquestion);
    List<FeedbackQuestionsOption> GetFeedbackQuestionOptionsById(Guid courseFeedbackQuestionId);
    void RemoveFeedbackQuestionOptions(List<FeedbackQuestionsOption> options);
    void DeleteFeedbackQuestion(CourseFeedbackQuestion coursefeedbackquestion);
    List<CourseFeedbackQuestionNoViewModel> GetFeedbackQuestionsByCourseId(Guid courseId);
    int GetNextFeedbackQuestionNo(Guid courseId);
}
