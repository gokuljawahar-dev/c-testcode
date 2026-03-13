namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IQuizQuestionJsonRepository
{
    Task<List<QuizQuestion>> AddQuestionsAsync(List<QuizQuestion> questions);
    Task AddOptionsAsync(List<QuestionOption> questionOptions, Guid quizQuestionId);
    Task<int> GetNextQuestionNoAsync(Guid quizId);
}
