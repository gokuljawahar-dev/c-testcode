namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IQuizRepository
{
    Quiz GetQuizById(Guid quizId);
    IEnumerable<Quiz> GetAllQuizzes();
    void AddQuiz(Quiz quiz);
    void UpdateQuiz(Quiz quiz);
    void DeleteQuiz(Quiz quiz);
    Topic GetTopicById(Guid topicId);
    Quiz GetQuizByTopicId(Guid topicId);

    //

    IEnumerable<QuizFeedbackQuestion> GetQuizFeedbackQuestionsByQuizId(Guid quizId);

    //new

    IEnumerable<LearnerAttempt> GetLearnerAttemptsByQuizId(Guid quizId);
    void DeleteLearnerAttempt(LearnerAttempt attempt);
}
