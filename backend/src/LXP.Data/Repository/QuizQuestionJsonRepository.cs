namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class QuizQuestionJsonRepository(LXPDbContext dbContext) : IQuizQuestionJsonRepository
{
    private readonly LXPDbContext _dbContext = dbContext;

    public async Task<List<QuizQuestion>> AddQuestionsAsync(List<QuizQuestion> questions)
    {
        await this._dbContext.QuizQuestions.AddRangeAsync(questions);
        await this._dbContext.SaveChangesAsync();
        return questions;
    }

    public async Task AddOptionsAsync(List<QuestionOption> questionOptions, Guid quizQuestionId)
    {
        var existingQuestion = await this._dbContext.QuizQuestions.FirstOrDefaultAsync(q =>
            q.QuizQuestionId == quizQuestionId
        );
        if (existingQuestion != null)
        {
            foreach (var option in questionOptions)
            {
                option.QuizQuestionId = quizQuestionId;
                this._dbContext.QuestionOptions.Add(option);
            }
            await this._dbContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Quiz question with ID {quizQuestionId} does not exist.");
        }
    }

    public async Task<int> GetNextQuestionNoAsync(Guid quizId)
    {
        try
        {
            var count = await this
                ._dbContext.QuizQuestions.Where(q => q.QuizId == quizId)
                .CountAsync();

            return count + 1;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "An error occurred while retrieving the next question number asynchronously.",
                ex
            );
        }
    }
}
