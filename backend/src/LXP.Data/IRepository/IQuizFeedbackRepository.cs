namespace LXP.Data.IRepository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;

public interface IQuizFeedbackRepository
{
    void AddFeedbackQuestion(QuizFeedbackQuestion questionEntity);
    void AddFeedbackQuestionOptions(List<FeedbackQuestionsOption> options);
    List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions();
    List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId);
    int GetNextFeedbackQuestionNo(Guid quizId);
    QuizFeedbackQuestion GetFeedbackQuestionEntityById(Guid quizFeedbackQuestionId);
    void UpdateFeedbackQuestion(QuizFeedbackQuestion questionEntity);
    void DeleteFeedbackQuestion(QuizFeedbackQuestion questionEntity);
    List<FeedbackQuestionsOption> GetFeedbackQuestionOptions(Guid quizFeedbackQuestionId);
    void DeleteFeedbackQuestionOptions(List<FeedbackQuestionsOption> options);
    void DeleteFeedbackResponses(List<FeedbackResponse> responses);
    List<FeedbackResponse> GetFeedbackResponsesByQuestionId(Guid quizFeedbackQuestionId);
}
