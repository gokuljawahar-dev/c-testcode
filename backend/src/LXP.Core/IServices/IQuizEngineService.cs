namespace LXP.Core.IServices;

using LXP.Common.ViewModels.QuizEngineViewModel;

public interface IQuizEngineService
{
    Task<ViewQuizDetailsViewModel> GetQuizByIdAsync(Guid quizId);
    Task<IEnumerable<QuizEngineQuestionViewModel>> GetQuestionsForQuizAsync(Guid quizId);
    Task<Guid> StartQuizAttemptAsync(Guid learnerId, Guid quizId);
    Task SubmitAnswerAsync(AnswerSubmissionModel answerSubmissionModel);
    Task<Guid> RetakeQuizAsync(Guid learnerId, Guid quizId);
    Task<ViewQuizDetailsViewModel> GetQuizDetailsByTopicIdAsync(Guid topicId);
    Task SubmitQuizAttemptAsync(Guid attemptId);
    Task<LearnerQuizAttemptViewModel> GetLearnerQuizAttemptAsync(Guid attemptId);
    Task<LearnerQuizAttemptResultViewModel> GetLearnerQuizAttemptResultAsync(Guid attemptId);

    //new
    Task<LearnerQuizStatusViewModel> GetLearnerQuizStatusAsync(Guid learnerId, Guid quizId);
    Task<LearnerPassStatusViewModel> CheckLearnerPassStatusAsync(Guid learnerAttemptId);
}
