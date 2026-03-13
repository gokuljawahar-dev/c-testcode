namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class QuizRepository(LXPDbContext dbContext) : IQuizRepository
{
    private readonly LXPDbContext _LXPDbContext = dbContext;

    public void AddQuiz(Quiz quiz)
    {
        this._LXPDbContext.Quizzes.Add(quiz);
        this._LXPDbContext.SaveChanges();
    }

    public Quiz GetQuizById(Guid quizId) => this._LXPDbContext.Quizzes.Find(quizId);

    public IEnumerable<Quiz> GetAllQuizzes() => this._LXPDbContext.Quizzes.ToList();

    public void UpdateQuiz(Quiz quiz)
    {
        this._LXPDbContext.Quizzes.Update(quiz);
        this._LXPDbContext.SaveChanges();
    }

    public void DeleteQuiz(Quiz quiz)
    {
        this._LXPDbContext.Quizzes.Remove(quiz);
        this._LXPDbContext.SaveChanges();
    }

    public Topic GetTopicById(Guid topicId) =>
        this._LXPDbContext.Topics.FirstOrDefault(t => t.TopicId == topicId);

    public Quiz GetQuizByTopicId(Guid topicId) =>
        this._LXPDbContext.Quizzes.FirstOrDefault(q => q.TopicId == topicId);

    //
    public IEnumerable<QuizFeedbackQuestion> GetQuizFeedbackQuestionsByQuizId(Guid quizId) =>
        this._LXPDbContext.QuizFeedbackQuestions.Where(q => q.QuizId == quizId).ToList();

    //new


    public IEnumerable<LearnerAttempt> GetLearnerAttemptsByQuizId(Guid quizId) =>
        this._LXPDbContext.LearnerAttempts.Where(a => a.QuizId == quizId).ToList();

    public void DeleteLearnerAttempt(LearnerAttempt attempt)
    {
        var learnerAnswers = this._LXPDbContext.LearnerAnswers.Where(a =>
            a.LearnerAttemptId == attempt.LearnerAttemptId
        );
        this._LXPDbContext.LearnerAnswers.RemoveRange(learnerAnswers);

        this._LXPDbContext.LearnerAttempts.Remove(attempt);
        this._LXPDbContext.SaveChanges();
    }
}
