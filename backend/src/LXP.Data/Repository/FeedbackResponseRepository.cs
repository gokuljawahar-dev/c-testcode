namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class FeedbackResponseRepository(LXPDbContext context) : IFeedbackResponseRepository
{
    private readonly LXPDbContext _context = context;

    public QuizFeedbackQuestion GetQuizFeedbackQuestion(Guid quizFeedbackQuestionId) =>
        this._context.QuizFeedbackQuestions.FirstOrDefault(q =>
            q.QuizFeedbackQuestionId == quizFeedbackQuestionId
        );

    public TopicFeedbackQuestion GetTopicFeedbackQuestion(Guid topicFeedbackQuestionId) =>
        this._context.TopicFeedbackQuestions.FirstOrDefault(q =>
            q.TopicFeedbackQuestionId == topicFeedbackQuestionId
        );

    public Learner GetLearner(Guid learnerId) =>
        this._context.Learners.FirstOrDefault(l => l.LearnerId == learnerId);

    public FeedbackResponse GetExistingQuizFeedbackResponse(
        Guid quizFeedbackQuestionId,
        Guid learnerId
    ) =>
        this._context.FeedbackResponses.FirstOrDefault(r =>
            r.QuizFeedbackQuestionId == quizFeedbackQuestionId && r.LearnerId == learnerId
        );

    public FeedbackResponse GetExistingTopicFeedbackResponse(
        Guid topicFeedbackQuestionId,
        Guid learnerId
    ) =>
        this._context.FeedbackResponses.FirstOrDefault(r =>
            r.TopicFeedbackQuestionId == topicFeedbackQuestionId && r.LearnerId == learnerId
        );

    public void AddFeedbackResponse(FeedbackResponse feedbackResponse)
    {
        this._context.FeedbackResponses.Add(feedbackResponse);
        this._context.SaveChanges();
    }

    public void AddFeedbackResponses(IEnumerable<FeedbackResponse> feedbackResponses)
    {
        this._context.FeedbackResponses.AddRange(feedbackResponses);
        this._context.SaveChanges();
    }

    //         _context.FeedbackQuestionsOptions.FirstOrDefault(o =>
    //             o.QuizFeedbackQuestionId == questionId
    //             && o.OptionText.ToLower() == optionText.ToLower()
    //         )
    //         ?? _context.FeedbackQuestionsOptions.FirstOrDefault(o =>
    //             o.TopicFeedbackQuestionId == questionId
    //             && o.OptionText.ToLower() == optionText.ToLower()
    //         );


    public Guid? GetOptionIdByText(Guid questionId, string optionText)
    {
        var option =
            this._context.FeedbackQuestionsOptions.FirstOrDefault(o =>
                o.QuizFeedbackQuestionId == questionId
                && o.OptionText.Equals(optionText, StringComparison.OrdinalIgnoreCase)
            )
            ?? this._context.FeedbackQuestionsOptions.FirstOrDefault(o =>
                o.TopicFeedbackQuestionId == questionId
                && o.OptionText.Equals(optionText, StringComparison.OrdinalIgnoreCase)
            )
            ?? this._context.FeedbackQuestionsOptions.FirstOrDefault(o =>
                o.CourseFeedbackQuestionId == questionId
                && o.OptionText.Equals(optionText, StringComparison.OrdinalIgnoreCase)
            );

        return option?.FeedbackQuestionOptionId;
    } // new code

    //new bug fix
    public void DeleteFeedbackResponsesByQuizQuestionId(Guid quizFeedbackQuestionId)
    {
        var responses = this
            ._context.FeedbackResponses.Where(r =>
                r.QuizFeedbackQuestionId == quizFeedbackQuestionId
            )
            .ToList();
        this._context.FeedbackResponses.RemoveRange(responses);
        this._context.SaveChanges();
    }

    public void DeleteFeedbackResponsesByTopicQuestionId(Guid topicFeedbackQuestionId)
    {
        var responses = this
            ._context.FeedbackResponses.Where(r =>
                r.TopicFeedbackQuestionId == topicFeedbackQuestionId
            )
            .ToList();
        this._context.FeedbackResponses.RemoveRange(responses);
        this._context.SaveChanges();
    }

    // new

    public IEnumerable<QuizFeedbackQuestion> GetQuizFeedbackQuestions(Guid quizId) =>
        this._context.QuizFeedbackQuestions.Where(q => q.QuizId == quizId).ToList();

    public IEnumerable<FeedbackResponse> GetQuizFeedbackResponsesByLearner(
        Guid learnerId,
        Guid quizId
    ) =>
        this
            ._context.FeedbackResponses.Where(r =>
                r.LearnerId == learnerId && r.QuizFeedbackQuestion.QuizId == quizId
            )
            .ToList();

    public IEnumerable<TopicFeedbackQuestion> GetTopicFeedbackQuestions(Guid topicId) =>
        this._context.TopicFeedbackQuestions.Where(q => q.TopicId == topicId).ToList();

    public IEnumerable<FeedbackResponse> GetTopicFeedbackResponsesByLearner(
        Guid learnerId,
        Guid topicId
    ) =>
        this
            ._context.FeedbackResponses.Where(r =>
                r.LearnerId == learnerId && r.TopicFeedbackQuestion.TopicId == topicId
            )
            .ToList();

    public CourseFeedbackQuestion GetCourseFeedbackQuestion(Guid courseFeedbackQuestionId) =>
        this._context.CourseFeedbackQuestions.FirstOrDefault(q =>
            q.CourseFeedbackQuestionId == courseFeedbackQuestionId
        );

    public FeedbackResponse GetExistingCourseFeedbackResponse(
        Guid courseFeedbackQuestionId,
        Guid learnerId
    ) =>
        this._context.FeedbackResponses.FirstOrDefault(r =>
            r.CourseFeedbackQuestionId == courseFeedbackQuestionId && r.LearnerId == learnerId
        );

    public IEnumerable<CourseFeedbackQuestion> GetCourseFeedbackQuestions(Guid courseId) =>
        this._context.CourseFeedbackQuestions.Where(q => q.CourseId == courseId).ToList();

    public IEnumerable<FeedbackResponse> GetCourseFeedbackResponsesByLearner(
        Guid learnerId,
        Guid courseId
    ) =>
        this
            ._context.FeedbackResponses.Where(r =>
                r.LearnerId == learnerId && r.CourseFeedbackQuestion.CourseId == courseId
            )
            .ToList();
}
