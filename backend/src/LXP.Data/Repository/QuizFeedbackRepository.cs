namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;
using LXP.Data.IRepository;

public class QuizFeedbackRepository(LXPDbContext dbContext) : IQuizFeedbackRepository
{
    private readonly LXPDbContext _dbContext = dbContext;

    public void AddFeedbackQuestion(QuizFeedbackQuestion questionEntity)
    {
        this._dbContext.QuizFeedbackQuestions.Add(questionEntity);
        this._dbContext.SaveChanges();
    }

    public void AddFeedbackQuestionOptions(List<FeedbackQuestionsOption> options)
    {
        this._dbContext.FeedbackQuestionsOptions.AddRange(options);
        this._dbContext.SaveChanges();
    }

    //             .QuizFeedbackQuestions.Select(q => new QuizfeedbackquestionNoViewModel
    //                 QuizFeedbackQuestionId = q.QuizFeedbackQuestionId,
    //                 QuizId = q.QuizId,
    //                 QuestionNo = q.QuestionNo,
    //                 Question = q.Question,
    //                 QuestionType = q.QuestionType,
    //                 Options =

    //                 [
    //                     .. this._dbContext
    //                                         .FeedbackQuestionsOptions.Where(o =>
    //                                             o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId
    //                                         )
    //                                         .Select(o => new QuizFeedbackQuestionsOptionViewModel
    //                                             OptionText = o.OptionText,
    // ,
    //                 ]
    //             .ToList();
    public List<QuizfeedbackquestionNoViewModel> GetAllFeedbackQuestions()
    {
        var questions = this
            ._dbContext.QuizFeedbackQuestions.Select(q => new
            {
                q.QuizFeedbackQuestionId,
                q.QuizId,
                q.QuestionNo,
                q.Question,
                q.QuestionType,
                Options = this
                    ._dbContext.FeedbackQuestionsOptions.Where(o =>
                        o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId
                    )
                    .Select(o => new QuizFeedbackQuestionsOptionViewModel
                    {
                        OptionText = o.OptionText
                    })
                    .ToList()
            })
            .ToList();

        var result = questions
            .Select(q => new QuizfeedbackquestionNoViewModel
            {
                QuizFeedbackQuestionId = q.QuizFeedbackQuestionId,
                QuizId = q.QuizId,
                QuestionNo = q.QuestionNo,
                Question = q.Question,
                QuestionType = q.QuestionType,
                Options = q.Options
            })
            .ToList();

        return result;
    }

    //             .QuizFeedbackQuestions.Where(q => q.QuizId == quizId)
    //             .Select(q => new QuizfeedbackquestionNoViewModel
    //                 QuizFeedbackQuestionId = q.QuizFeedbackQuestionId,
    //                 QuizId = q.QuizId,
    //                 QuestionNo = q.QuestionNo,
    //                 Question = q.Question,
    //                 QuestionType = q.QuestionType,
    //                 Options =

    //                 [
    //                     .. this._dbContext
    //                                         .FeedbackQuestionsOptions.Where(o =>
    //                                             o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId
    //                                         )
    //                                         .Select(o => new QuizFeedbackQuestionsOptionViewModel
    //                                             OptionText = o.OptionText
    // ,
    //                 ]
    //             .ToList();
    public List<QuizfeedbackquestionNoViewModel> GetFeedbackQuestionsByQuizId(Guid quizId)
    {
        var questions = this
            ._dbContext.QuizFeedbackQuestions.Where(q => q.QuizId == quizId)
            .Select(q => new
            {
                q.QuizFeedbackQuestionId,
                q.QuizId,
                q.QuestionNo,
                q.Question,
                q.QuestionType,
                Options = this
                    ._dbContext.FeedbackQuestionsOptions.Where(o =>
                        o.QuizFeedbackQuestionId == q.QuizFeedbackQuestionId
                    )
                    .Select(o => new QuizFeedbackQuestionsOptionViewModel
                    {
                        OptionText = o.OptionText
                    })
                    .ToList()
            })
            .ToList();

        var result = questions
            .Select(q => new QuizfeedbackquestionNoViewModel
            {
                QuizFeedbackQuestionId = q.QuizFeedbackQuestionId,
                QuizId = q.QuizId,
                QuestionNo = q.QuestionNo,
                Question = q.Question,
                QuestionType = q.QuestionType,
                Options = q.Options
            })
            .ToList();

        return result;
    }

    public int GetNextFeedbackQuestionNo(Guid quizId)
    {
        var lastQuestion = this
            ._dbContext.QuizFeedbackQuestions.Where(q => q.QuizId == quizId)
            .OrderByDescending(q => q.QuestionNo)
            .FirstOrDefault();
        return lastQuestion != null ? lastQuestion.QuestionNo + 1 : 1;
    }

    public QuizFeedbackQuestion GetFeedbackQuestionEntityById(Guid quizFeedbackQuestionId) =>
        this._dbContext.QuizFeedbackQuestions.FirstOrDefault(q =>
            q.QuizFeedbackQuestionId == quizFeedbackQuestionId
        );

    public void UpdateFeedbackQuestion(QuizFeedbackQuestion questionEntity)
    {
        this._dbContext.QuizFeedbackQuestions.Update(questionEntity);
        this._dbContext.SaveChanges();
    }

    public void DeleteFeedbackQuestion(QuizFeedbackQuestion questionEntity)
    {
        this._dbContext.QuizFeedbackQuestions.Remove(questionEntity);
        this._dbContext.SaveChanges();
    }

    public List<FeedbackQuestionsOption> GetFeedbackQuestionOptions(Guid quizFeedbackQuestionId) =>
        this
            ._dbContext.FeedbackQuestionsOptions.Where(o =>
                o.QuizFeedbackQuestionId == quizFeedbackQuestionId
            )
            .ToList();

    public void DeleteFeedbackQuestionOptions(List<FeedbackQuestionsOption> options)
    {
        this._dbContext.FeedbackQuestionsOptions.RemoveRange(options);
        this._dbContext.SaveChanges();
    }

    public void DeleteFeedbackResponses(List<FeedbackResponse> responses)
    {
        this._dbContext.FeedbackResponses.RemoveRange(responses);
        this._dbContext.SaveChanges();
    }

    public List<FeedbackResponse> GetFeedbackResponsesByQuestionId(Guid quizFeedbackQuestionId) =>
        this
            ._dbContext.FeedbackResponses.Where(r =>
                r.QuizFeedbackQuestionId == quizFeedbackQuestionId
            )
            .ToList();
}
