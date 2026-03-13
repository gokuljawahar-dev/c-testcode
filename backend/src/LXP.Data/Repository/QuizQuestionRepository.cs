namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class QuizQuestionRepository(LXPDbContext dbContext) : IQuizQuestionRepository
{
    private readonly LXPDbContext _LXPDbContext =
        dbContext
        ?? throw new ArgumentNullException(nameof(dbContext), "DB context cannot be null.");

    public async Task<Guid> AddQuestionAsync(
        QuizQuestionViewModel quizQuestion,
        List<QuestionOptionViewModel> options
    )
    {
        var quizQuestionEntity = new QuizQuestion
        {
            QuizId = quizQuestion.QuizId,
            Question = quizQuestion.Question,
            QuestionType = quizQuestion.QuestionType,
            QuestionNo = await this.GetNextQuestionNoAsync(quizQuestion.QuizId),
            CreatedBy = "SystemUser",
            CreatedAt = DateTime.Now
        };

        await this._LXPDbContext.QuizQuestions.AddAsync(quizQuestionEntity);
        await this._LXPDbContext.SaveChangesAsync();

        foreach (var option in options)
        {
            var questionOptionEntity = new QuestionOption
            {
                QuizQuestionId = quizQuestionEntity.QuizQuestionId,
                Option = option.Option,
                IsCorrect = option.IsCorrect,
                CreatedBy = "SystemUser",
                CreatedAt = DateTime.Now
            };

            await this._LXPDbContext.QuestionOptions.AddAsync(questionOptionEntity);
        }

        await this._LXPDbContext.SaveChangesAsync();

        return quizQuestionEntity.QuizQuestionId;
    }

    public async Task<bool> UpdateQuestionAsync(
        Guid quizQuestionId,
        QuizQuestionViewModel quizQuestion,
        List<QuestionOptionViewModel> options
    )
    {
        var quizQuestionEntity = await this._LXPDbContext.QuizQuestions.FindAsync(quizQuestionId);
        if (quizQuestionEntity == null)
        {
            return false;
        }

        quizQuestionEntity.Question = quizQuestion.Question;

        var existingOptions = this
            ._LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId)
            .ToList();
        this._LXPDbContext.QuestionOptions.RemoveRange(existingOptions);

        foreach (var option in options)
        {
            var questionOptionEntity = new QuestionOption
            {
                QuizQuestionId = quizQuestionEntity.QuizQuestionId,
                Option = option.Option,
                IsCorrect = option.IsCorrect,
                CreatedBy = quizQuestionEntity.CreatedBy,
                CreatedAt = quizQuestionEntity.CreatedAt
            };

            await this._LXPDbContext.QuestionOptions.AddAsync(questionOptionEntity);
        }

        await this._LXPDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteQuestionAsync(Guid quizQuestionId)
    {
        var quizQuestionEntity = await this._LXPDbContext.QuizQuestions.FindAsync(quizQuestionId);
        if (quizQuestionEntity == null)
        {
            return false;
        }

        this._LXPDbContext.QuestionOptions.RemoveRange(
            this._LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == quizQuestionId)
        );
        this._LXPDbContext.QuizQuestions.Remove(quizQuestionEntity);
        await this._LXPDbContext.SaveChangesAsync();

        return true;
    }

    //             .QuizQuestions.Select(q => new QuizQuestionNoViewModel
    //                 QuizId = q.QuizId,
    //                 QuizQuestionId = q.QuizQuestionId,
    //                 Question = q.Question,
    //                 QuestionType = q.QuestionType,
    //                 QuestionNo = q.QuestionNo,
    //                 Options =

    //                 [
    //                     .. this._LXPDbContext
    //                                         .QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
    //                                         .Select(o => new QuestionOptionViewModel
    //                                             Option = o.Option,
    //                                             IsCorrect = o.IsCorrect
    // ,
    //                 ]
    //             .ToListAsync();
    public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync()
    {
        var questions = await this
            ._LXPDbContext.QuizQuestions.Select(q => new
            {
                q.QuizId,
                q.QuizQuestionId,
                q.Question,
                q.QuestionType,
                q.QuestionNo,
                Options = this
                    ._LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
                    .Select(o => new QuestionOptionViewModel
                    {
                        Option = o.Option,
                        IsCorrect = o.IsCorrect
                    })
                    .ToList()
            })
            .ToListAsync();

        var result = questions
            .Select(q => new QuizQuestionNoViewModel
            {
                QuizId = q.QuizId,
                QuizQuestionId = q.QuizQuestionId,
                Question = q.Question,
                QuestionType = q.QuestionType,
                QuestionNo = q.QuestionNo,
                Options = q.Options
            })
            .ToList();

        return result;
    }

    //             .QuizQuestions.Where(q => q.QuizId == quizId)
    //             .Select(q => new QuizQuestionNoViewModel
    //                 QuizId = q.QuizId,
    //                 QuizQuestionId = q.QuizQuestionId,
    //                 Question = q.Question,
    //                 QuestionType = q.QuestionType,
    //                 QuestionNo = q.QuestionNo,
    //                 Options =

    //                 [
    //                     .. this._LXPDbContext
    //                                         .QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
    //                                         .Select(o => new QuestionOptionViewModel
    //                                             Option = o.Option,
    //                                             IsCorrect = o.IsCorrect
    // ,
    //                 ]
    //             .ToListAsync();
    public async Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId)
    {
        var questions = await this
            ._LXPDbContext.QuizQuestions.Where(q => q.QuizId == quizId)
            .Select(q => new
            {
                q.QuizId,
                q.QuizQuestionId,
                q.Question,
                q.QuestionType,
                q.QuestionNo,
                Options = this
                    ._LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
                    .Select(o => new QuestionOptionViewModel
                    {
                        Option = o.Option,
                        IsCorrect = o.IsCorrect
                    })
                    .ToList()
            })
            .ToListAsync();

        var result = questions
            .Select(q => new QuizQuestionNoViewModel
            {
                QuizId = q.QuizId,
                QuizQuestionId = q.QuizQuestionId,
                Question = q.Question,
                QuestionType = q.QuestionType,
                QuestionNo = q.QuestionNo,
                Options = q.Options
            })
            .ToList();

        return result;
    }

    public async Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId)
    {
        var quizQuestion = await this
            ._LXPDbContext.QuizQuestions.Where(q => q.QuizQuestionId == quizQuestionId)
            .Select(q => new
            {
                q.QuizId,
                q.QuizQuestionId,
                q.Question,
                q.QuestionType,
                q.QuestionNo,
                Options = this
                    ._LXPDbContext.QuestionOptions.Where(o => o.QuizQuestionId == q.QuizQuestionId)
                    .Select(o => new QuestionOptionViewModel
                    {
                        Option = o.Option,
                        IsCorrect = o.IsCorrect
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (quizQuestion == null)
        {
            return null;
        }

        return new QuizQuestionNoViewModel
        {
            QuizId = quizQuestion.QuizId,
            QuizQuestionId = quizQuestion.QuizQuestionId,
            Question = quizQuestion.Question,
            QuestionType = quizQuestion.QuestionType,
            QuestionNo = quizQuestion.QuestionNo,
            Options = quizQuestion.Options ?? []
        };
    }

    public async Task<int> GetNextQuestionNoAsync(Guid quizId)
    {
        var count = await this
            ._LXPDbContext.QuizQuestions.Where(q => q.QuizId == quizId)
            .CountAsync();
        return count + 1;
    }

    public void ReorderQuestionNos(Guid quizId, int deletedQuestionNo)
    {
        var subsequentQuestions = this
            ._LXPDbContext.QuizQuestions.Where(q =>
                q.QuizId == quizId && q.QuestionNo > deletedQuestionNo
            )
            .ToList();
        foreach (var question in subsequentQuestions)
        {
            question.QuestionNo--;
        }
        this._LXPDbContext.SaveChanges();
    }
}
