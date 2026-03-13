namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class CourseFeedbackRepository(LXPDbContext context) : ICourseFeedbackRepository
{
    private readonly LXPDbContext _context = context;

    public void AddFeedbackQuestion(CourseFeedbackQuestion coursefeedbackquestion)
    {
        this._context.CourseFeedbackQuestions.Add(coursefeedbackquestion);
        this._context.SaveChanges();
    }

    public void AddFeedbackQuestionOptions(List<FeedbackQuestionsOption> options)
    {
        this._context.FeedbackQuestionsOptions.AddRange(options);
        this._context.SaveChanges();
    }

    public List<CourseFeedbackQuestionNoViewModel> GetAllFeedbackQuestions()
    {
        var feedbackQuestions = this
            ._context.CourseFeedbackQuestions.Include(q => q.FeedbackQuestionsOptions)
            .ToList();

        return feedbackQuestions
            .Select(q => new CourseFeedbackQuestionNoViewModel
            {
                CourseFeedbackQuestionId = q.CourseFeedbackQuestionId,
                CourseId = q.CourseId,
                QuestionNo = q.QuestionNo,
                Question = q.Question,
                QuestionType = q.QuestionType,
                Options = q
                    .FeedbackQuestionsOptions?.Select(
                        o => new CourseFeedbackQuestionsOptionViewModel
                        {
                            OptionText = o.OptionText
                        }
                    )
                    .ToList()
            })
            .ToList();
    }

    public CourseFeedbackQuestionNoViewModel GetFeedbackQuestionById(Guid courseFeedbackQuestionId)
    {
        var question = this
            ._context.CourseFeedbackQuestions.Include(q => q.FeedbackQuestionsOptions)
            .FirstOrDefault(q => q.CourseFeedbackQuestionId == courseFeedbackQuestionId);

        if (question == null)
        {
            return null;
        }

        return new CourseFeedbackQuestionNoViewModel
        {
            CourseFeedbackQuestionId = question.CourseFeedbackQuestionId,
            CourseId = question.CourseId,
            QuestionNo = question.QuestionNo,
            Question = question.Question,
            QuestionType = question.QuestionType,
            Options = question
                .FeedbackQuestionsOptions?.Select(o => new CourseFeedbackQuestionsOptionViewModel
                {
                    OptionText = o.OptionText
                })
                .ToList()
        };
    }

    public CourseFeedbackQuestion GetCourseFeedbackQuestionEntityById(
        Guid courseFeedbackQuestionId
    ) =>
        this
            ._context.CourseFeedbackQuestions.Include(q => q.FeedbackQuestionsOptions)
            .FirstOrDefault(q => q.CourseFeedbackQuestionId == courseFeedbackQuestionId);

    public void UpdateFeedbackQuestion(CourseFeedbackQuestion coursefeedbackquestion)
    {
        this._context.CourseFeedbackQuestions.Update(coursefeedbackquestion);
        this._context.SaveChanges();
    }

    public List<FeedbackQuestionsOption> GetFeedbackQuestionOptionsById(
        Guid courseFeedbackQuestionId
    ) =>
        this
            ._context.FeedbackQuestionsOptions.Where(o =>
                o.CourseFeedbackQuestionId == courseFeedbackQuestionId
            )
            .ToList();

    public void RemoveFeedbackQuestionOptions(List<FeedbackQuestionsOption> options)
    {
        this._context.FeedbackQuestionsOptions.RemoveRange(options);
        this._context.SaveChanges();
    }

    public void DeleteFeedbackQuestion(CourseFeedbackQuestion coursefeedbackquestion)
    {
        this._context.CourseFeedbackQuestions.Remove(coursefeedbackquestion);
        this._context.SaveChanges();
    }

    public List<CourseFeedbackQuestionNoViewModel> GetFeedbackQuestionsByCourseId(Guid courseId)
    {
        var feedbackQuestions = this
            ._context.CourseFeedbackQuestions.Where(q => q.CourseId == courseId)
            .Include(q => q.FeedbackQuestionsOptions)
            .ToList();

        return feedbackQuestions
            .Select(q => new CourseFeedbackQuestionNoViewModel
            {
                CourseFeedbackQuestionId = q.CourseFeedbackQuestionId,
                CourseId = q.CourseId,
                QuestionNo = q.QuestionNo,
                Question = q.Question,
                QuestionType = q.QuestionType,
                Options = q
                    .FeedbackQuestionsOptions?.Select(
                        o => new CourseFeedbackQuestionsOptionViewModel
                        {
                            OptionText = o.OptionText
                        }
                    )
                    .ToList()
            })
            .ToList();
    }

    public int GetNextFeedbackQuestionNo(Guid courseId) =>
        this._context.CourseFeedbackQuestions.Where(q => q.CourseId == courseId)
            .Max(q => (int?)q.QuestionNo) + 1
        ?? 1;
}
