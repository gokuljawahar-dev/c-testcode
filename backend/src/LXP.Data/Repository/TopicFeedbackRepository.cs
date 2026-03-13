namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.TopicFeedbackQuestionViewModel;
using LXP.Data.IRepository;

public class TopicFeedbackRepository(LXPDbContext dbContext) : ITopicFeedbackRepository
{
    private readonly LXPDbContext _dbContext = dbContext;

    public void AddFeedbackQuestion(TopicFeedbackQuestion questionEntity)
    {
        this._dbContext.TopicFeedbackQuestions.Add(questionEntity);
        this._dbContext.SaveChanges();
    }

    public void AddFeedbackQuestionOptions(List<FeedbackQuestionsOption> options)
    {
        this._dbContext.FeedbackQuestionsOptions.AddRange(options);
        this._dbContext.SaveChanges();
    }

    //             .TopicFeedbackQuestions.Select(q => new TopicFeedbackQuestionNoViewModel
    //                 TopicFeedbackQuestionId = q.TopicFeedbackQuestionId,
    //                 TopicId = q.TopicId,
    //                 QuestionNo = q.QuestionNo,
    //                 Question = q.Question,
    //                 QuestionType = q.QuestionType,
    //                 Options =

    //                 [
    //                     .. this._dbContext
    //                                         .FeedbackQuestionsOptions.Where(o =>
    //                                             o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId
    //                                         )
    //                                         .Select(o => new TopicFeedbackQuestionsOptionViewModel
    //                                             OptionText = o.OptionText
    // ,
    //                 ]
    //             .ToList();
    public List<TopicFeedbackQuestionNoViewModel> GetAllFeedbackQuestions()
    {
        var questions = this
            ._dbContext.TopicFeedbackQuestions.Select(q => new
            {
                q.TopicFeedbackQuestionId,
                q.TopicId,
                q.QuestionNo,
                q.Question,
                q.QuestionType,
                Options = this
                    ._dbContext.FeedbackQuestionsOptions.Where(o =>
                        o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId
                    )
                    .Select(o => new TopicFeedbackQuestionsOptionViewModel
                    {
                        OptionText = o.OptionText
                    })
                    .ToList()
            })
            .ToList();

        var result = questions
            .Select(q => new TopicFeedbackQuestionNoViewModel
            {
                TopicFeedbackQuestionId = q.TopicFeedbackQuestionId,
                TopicId = q.TopicId,
                QuestionNo = q.QuestionNo,
                Question = q.Question,
                QuestionType = q.QuestionType,
                Options = q.Options
            })
            .ToList();

        return result;
    }

    //             .TopicFeedbackQuestions.Where(q => q.TopicId == topicId)
    //             .Select(q => new TopicFeedbackQuestionNoViewModel
    //                 TopicFeedbackQuestionId = q.TopicFeedbackQuestionId,
    //                 TopicId = q.TopicId,
    //                 QuestionNo = q.QuestionNo,
    //                 Question = q.Question,
    //                 QuestionType = q.QuestionType,
    //                 Options =

    //                 [
    //                     .. this._dbContext
    //                                         .FeedbackQuestionsOptions.Where(o =>
    //                                             o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId
    //                                         )
    //                                         .Select(o => new TopicFeedbackQuestionsOptionViewModel
    //                                             OptionText = o.OptionText
    // ,
    //                 ]
    //             .ToList();
    public List<TopicFeedbackQuestionNoViewModel> GetFeedbackQuestionsByTopicId(Guid topicId)
    {
        var questions = this
            ._dbContext.TopicFeedbackQuestions.Where(q => q.TopicId == topicId)
            .Select(q => new
            {
                q.TopicFeedbackQuestionId,
                q.TopicId,
                q.QuestionNo,
                q.Question,
                q.QuestionType,
                Options = this
                    ._dbContext.FeedbackQuestionsOptions.Where(o =>
                        o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId
                    )
                    .Select(o => new TopicFeedbackQuestionsOptionViewModel
                    {
                        OptionText = o.OptionText
                    })
                    .ToList()
            })
            .ToList();

        var result = questions
            .Select(q => new TopicFeedbackQuestionNoViewModel
            {
                TopicFeedbackQuestionId = q.TopicFeedbackQuestionId,
                TopicId = q.TopicId,
                QuestionNo = q.QuestionNo,
                Question = q.Question,
                QuestionType = q.QuestionType,
                Options = q.Options
            })
            .ToList();

        return result;
    }

    public int GetNextFeedbackQuestionNo(Guid topicId)
    {
        var lastQuestion = this
            ._dbContext.TopicFeedbackQuestions.Where(q => q.TopicId == topicId)
            .OrderByDescending(q => q.QuestionNo)
            .FirstOrDefault();
        return lastQuestion != null ? lastQuestion.QuestionNo + 1 : 1;
    }

    public TopicFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid topicFeedbackQuestionId)
    {
        var feedbackQuestion = this
            ._dbContext.TopicFeedbackQuestions.Where(q =>
                q.TopicFeedbackQuestionId == topicFeedbackQuestionId
            )
            .Select(q => new
            {
                q.TopicFeedbackQuestionId,
                q.TopicId,
                q.QuestionNo,
                q.Question,
                q.QuestionType,
                Options = this
                    ._dbContext.FeedbackQuestionsOptions.Where(o =>
                        o.TopicFeedbackQuestionId == q.TopicFeedbackQuestionId
                    )
                    .Select(o => new TopicFeedbackQuestionsOptionViewModel
                    {
                        OptionText = o.OptionText
                    })
                    .ToList()
            })
            .FirstOrDefault();

        if (feedbackQuestion == null)
        {
            return null;
        }

        return new TopicFeedbackQuestionNoViewModel
        {
            TopicFeedbackQuestionId = feedbackQuestion.TopicFeedbackQuestionId,
            TopicId = feedbackQuestion.TopicId,
            QuestionNo = feedbackQuestion.QuestionNo,
            Question = feedbackQuestion.Question,
            QuestionType = feedbackQuestion.QuestionType,
            Options = feedbackQuestion.Options ?? []
        };
    }

    public void UpdateFeedbackQuestion(TopicFeedbackQuestion questionEntity) =>
        this._dbContext.SaveChanges();

    public void RemoveFeedbackQuestion(TopicFeedbackQuestion questionEntity)
    {
        // Delete related FeedbackResponses first
        var relatedResponses = this
            ._dbContext.FeedbackResponses.Where(r =>
                r.TopicFeedbackQuestionId == questionEntity.TopicFeedbackQuestionId
            )
            .ToList();

        if (relatedResponses.Count != 0)
        {
            this._dbContext.FeedbackResponses.RemoveRange(relatedResponses);
        }

        // Delete the FeedbackQuestion itself
        this._dbContext.TopicFeedbackQuestions.Remove(questionEntity);
        this._dbContext.SaveChanges();
    }

    public void RemoveFeedbackQuestionOptions(List<FeedbackQuestionsOption> options)
    {
        this._dbContext.FeedbackQuestionsOptions.RemoveRange(options);
        this._dbContext.SaveChanges();
    }

    public void ReorderQuestionNos(Guid topicId, int deletedQuestionNo)
    {
        var questionsToUpdate = this
            ._dbContext.TopicFeedbackQuestions.Where(q =>
                q.TopicId == topicId && q.QuestionNo > deletedQuestionNo
            )
            .OrderBy(q => q.QuestionNo)
            .ToList();

        foreach (var question in questionsToUpdate)
        {
            question.QuestionNo--;
        }
        this._dbContext.SaveChanges();
    }

    public List<FeedbackQuestionsOption> GetFeedbackQuestionOptionsById(
        Guid topicFeedbackQuestionId
    ) =>
        this
            ._dbContext.FeedbackQuestionsOptions.Where(o =>
                o.TopicFeedbackQuestionId == topicFeedbackQuestionId
            )
            .ToList();

    public TopicFeedbackQuestion GetTopicFeedbackQuestionEntityById(Guid topicFeedbackQuestionId) =>
        this._dbContext.TopicFeedbackQuestions.FirstOrDefault(q =>
            q.TopicFeedbackQuestionId == topicFeedbackQuestionId
        );
}
