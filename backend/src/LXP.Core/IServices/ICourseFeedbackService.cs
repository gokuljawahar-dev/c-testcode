namespace LXP.Core.IServices;

using LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;

public interface ICourseFeedbackService
{
    Guid AddFeedbackQuestion(
        CourseFeedbackQuestionViewModel courseFeedbackQuestion,
        List<CourseFeedbackQuestionsOptionViewModel> options
    );
    List<CourseFeedbackQuestionNoViewModel> GetAllFeedbackQuestions();
    CourseFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid courseFeedbackQuestionId);
    bool UpdateFeedbackQuestion(
        Guid courseFeedbackQuestionId,
        CourseFeedbackQuestionViewModel courseFeedbackQuestion,
        List<CourseFeedbackQuestionsOptionViewModel> options
    );
    bool DeleteFeedbackQuestion(Guid courseFeedbackQuestionId);
    List<CourseFeedbackQuestionNoViewModel> GetFeedbackQuestionsByCourseId(Guid courseId);
}
