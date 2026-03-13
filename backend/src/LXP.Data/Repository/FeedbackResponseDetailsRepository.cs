namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class FeedbackResponseDetailsRepository(LXPDbContext context)
    : IFeedbackResponseDetailsRepository
{
    private readonly LXPDbContext _context = context;

    public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponses(Guid quizId)
    {
        var results = this
            ._context.FeedbackResponses.Include(r => r.QuizFeedbackQuestion)
            .Include(r => r.QuizFeedbackQuestion.FeedbackQuestionsOptions)
            .Include(r => r.Learner.LearnerProfiles)
            .Where(r => r.QuizFeedbackQuestion.QuizId == quizId)
            .Select(r => new
            {
                r.QuizFeedbackQuestionId,
                r.LearnerId,
                r.QuizFeedbackQuestion.Question,
                r.QuizFeedbackQuestion.QuestionType,
                r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .QuizFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null,
                LearnerName = r
                    .Learner.LearnerProfiles.Select(lp => lp.FirstName + " " + lp.LastName)
                    .FirstOrDefault()
            })
            .ToList();

        return results
            .Select(r => new QuizFeedbackResponseDetailsViewModel
            {
                QuizFeedbackQuestionId = r.QuizFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.Question,
                QuestionType = r.QuestionType,
                Response = r.Response ?? "N/A",
                OptionText = r.OptionText ?? "N/A",
                LearnerName = r.LearnerName ?? "N/A"
            })
            .ToList();
    }

    public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponses(Guid topicId)
    {
        var results = this
            ._context.FeedbackResponses.Include(r => r.TopicFeedbackQuestion)
            .Include(r => r.TopicFeedbackQuestion.FeedbackQuestionsOptions)
            .Include(r => r.Learner.LearnerProfiles)
            .Where(r => r.TopicFeedbackQuestion.TopicId == topicId)
            .Select(r => new
            {
                r.TopicFeedbackQuestionId,
                r.LearnerId,
                r.TopicFeedbackQuestion.Question,
                r.TopicFeedbackQuestion.QuestionType,
                r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .TopicFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null,
                LearnerName = r
                    .Learner.LearnerProfiles.Select(lp => lp.FirstName + " " + lp.LastName)
                    .FirstOrDefault()
            })
            .ToList();

        return results
            .Select(r => new TopicFeedbackResponseDetailsViewModel
            {
                TopicFeedbackQuestionId = r.TopicFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.Question,
                QuestionType = r.QuestionType,
                Response = r.Response ?? "N/A",
                OptionText = r.OptionText ?? "N/A",
                LearnerName = r.LearnerName ?? "N/A"
            })
            .ToList();
    }

    public List<QuizFeedbackResponseDetailsViewModel> GetQuizFeedbackResponsesByLearner(
        Guid quizId,
        Guid learnerId
    ) =>
        this
            ._context.FeedbackResponses.Include(r => r.QuizFeedbackQuestion)
            .Include(r => r.QuizFeedbackQuestion.FeedbackQuestionsOptions)
            .Where(r => r.QuizFeedbackQuestion.QuizId == quizId && r.LearnerId == learnerId)
            .Select(r => new QuizFeedbackResponseDetailsViewModel
            {
                QuizFeedbackQuestionId = r.QuizFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.QuizFeedbackQuestion.Question,
                QuestionType = r.QuizFeedbackQuestion.QuestionType,
                Response = r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .QuizFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null
            })
            .ToList();

    public List<TopicFeedbackResponseDetailsViewModel> GetTopicFeedbackResponsesByLearner(
        Guid topicId,
        Guid learnerId
    ) =>
        this
            ._context.FeedbackResponses.Include(r => r.TopicFeedbackQuestion)
            .Include(r => r.TopicFeedbackQuestion.FeedbackQuestionsOptions)
            .Where(r => r.TopicFeedbackQuestion.TopicId == topicId && r.LearnerId == learnerId)
            .Select(r => new TopicFeedbackResponseDetailsViewModel
            {
                TopicFeedbackQuestionId = r.TopicFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.TopicFeedbackQuestion.Question,
                QuestionType = r.TopicFeedbackQuestion.QuestionType,
                Response = r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .TopicFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null
            })
            .ToList();

    public List<QuizFeedbackResponseDetailsViewModel> GetAllQuizFeedbackResponses()
    {
        var results = this
            ._context.FeedbackResponses.Include(r => r.QuizFeedbackQuestion)
            .Include(r => r.QuizFeedbackQuestion.FeedbackQuestionsOptions)
            .Include(r => r.Learner.LearnerProfiles)
            .Include(r => r.QuizFeedbackQuestion.Quiz) // Include Quiz to get QuizName
            .Where(r => r.QuizFeedbackQuestion != null)
            .Select(r => new
            {
                r.QuizFeedbackQuestionId,
                r.LearnerId,
                r.QuizFeedbackQuestion.Question,
                r.QuizFeedbackQuestion.QuestionType,
                r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .QuizFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null,
                LearnerName = r
                    .Learner.LearnerProfiles.Select(lp => lp.FirstName + " " + lp.LastName)
                    .FirstOrDefault(),
                QuizName = r.QuizFeedbackQuestion.Quiz.NameOfQuiz // Get Quiz Name
            })
            .ToList();

        return results
            .Select(r => new QuizFeedbackResponseDetailsViewModel
            {
                QuizFeedbackQuestionId = r.QuizFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.Question,
                QuestionType = r.QuestionType,
                Response = r.Response ?? "N/A",
                OptionText = r.OptionText ?? "N/A",
                LearnerName = r.LearnerName ?? "N/A",
                QuizName = r.QuizName ?? "N/A" // Include Quiz Name
            })
            .ToList();
    }

    public List<TopicFeedbackResponseDetailsViewModel> GetAllTopicFeedbackResponses()
    {
        var results = this
            ._context.FeedbackResponses.Include(r => r.TopicFeedbackQuestion)
            .Include(r => r.TopicFeedbackQuestion.FeedbackQuestionsOptions)
            .Include(r => r.Learner.LearnerProfiles)
            .Include(r => r.TopicFeedbackQuestion.Topic) // Include Topic to get TopicName
            .Where(r => r.TopicFeedbackQuestion != null)
            .Select(r => new
            {
                r.TopicFeedbackQuestionId,
                r.LearnerId,
                r.TopicFeedbackQuestion.Question,
                r.TopicFeedbackQuestion.QuestionType,
                r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .TopicFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null,
                LearnerName = r
                    .Learner.LearnerProfiles.Select(lp => lp.FirstName + " " + lp.LastName)
                    .FirstOrDefault(),
                TopicName = r.TopicFeedbackQuestion.Topic.Name // Get Topic Name
            })
            .ToList();

        return results
            .Select(r => new TopicFeedbackResponseDetailsViewModel
            {
                TopicFeedbackQuestionId = r.TopicFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.Question,
                QuestionType = r.QuestionType,
                Response = r.Response ?? "N/A",
                OptionText = r.OptionText ?? "N/A",
                LearnerName = r.LearnerName ?? "N/A",
                TopicName = r.TopicName ?? "N" // Include Topic Name
            })
            .ToList();
    }

    public List<CourseFeedbackResponseDetailsViewModel> GetCourseFeedbackResponses(Guid courseId)
    {
        var results = this
            ._context.FeedbackResponses.Include(r => r.CourseFeedbackQuestion)
            .Include(r => r.CourseFeedbackQuestion.FeedbackQuestionsOptions)
            .Include(r => r.Learner.LearnerProfiles)
            .Where(r => r.CourseFeedbackQuestion.CourseId == courseId)
            .Select(r => new
            {
                r.CourseFeedbackQuestionId,
                r.LearnerId,
                r.CourseFeedbackQuestion.Question,
                r.CourseFeedbackQuestion.QuestionType,
                r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .CourseFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null,
                LearnerName = r
                    .Learner.LearnerProfiles.Select(lp => lp.FirstName + " " + lp.LastName)
                    .FirstOrDefault()
            })
            .ToList();

        return results
            .Select(r => new CourseFeedbackResponseDetailsViewModel
            {
                CourseFeedbackQuestionId = r.CourseFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.Question,
                QuestionType = r.QuestionType,
                Response = r.Response ?? "N/A",
                OptionText = r.OptionText ?? "N/A",
                LearnerName = r.LearnerName ?? "N/A"
            })
            .ToList();
    }

    public List<CourseFeedbackResponseDetailsViewModel> GetCourseFeedbackResponsesByLearner(
        Guid courseId,
        Guid learnerId
    ) =>
        this
            ._context.FeedbackResponses.Include(r => r.CourseFeedbackQuestion)
            .Include(r => r.CourseFeedbackQuestion.FeedbackQuestionsOptions)
            .Where(r => r.CourseFeedbackQuestion.CourseId == courseId && r.LearnerId == learnerId)
            .Select(r => new CourseFeedbackResponseDetailsViewModel
            {
                CourseFeedbackQuestionId = r.CourseFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.CourseFeedbackQuestion.Question,
                QuestionType = r.CourseFeedbackQuestion.QuestionType,
                Response = r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .CourseFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null
            })
            .ToList();

    public List<CourseFeedbackResponseDetailsViewModel> GetAllCourseFeedbackResponses()
    {
        var results = this
            ._context.FeedbackResponses.Include(r => r.CourseFeedbackQuestion)
            .Include(r => r.CourseFeedbackQuestion.FeedbackQuestionsOptions)
            .Include(r => r.Learner.LearnerProfiles)
            .Include(r => r.CourseFeedbackQuestion.Course) // Include Course
            .Select(r => new
            {
                r.CourseFeedbackQuestionId,
                r.LearnerId,
                r.CourseFeedbackQuestion.Question,
                r.CourseFeedbackQuestion.QuestionType,
                r.Response,
                OptionText = r.OptionId.HasValue
                    ? r
                        .CourseFeedbackQuestion.FeedbackQuestionsOptions.FirstOrDefault(o =>
                            o.FeedbackQuestionOptionId == r.OptionId.Value
                        )
                        .OptionText
                    : null,
                LearnerName = r
                    .Learner.LearnerProfiles.Select(lp => lp.FirstName + " " + lp.LastName)
                    .FirstOrDefault(),
                CourseName = r.CourseFeedbackQuestion.Course.Title // Added Course Title
            })
            .ToList();

        return results
            .Select(r => new CourseFeedbackResponseDetailsViewModel
            {
                CourseFeedbackQuestionId = r.CourseFeedbackQuestionId ?? Guid.Empty,
                LearnerId = r.LearnerId,
                Question = r.Question,
                QuestionType = r.QuestionType,
                Response = r.Response ?? "N/A",
                OptionText = r.OptionText ?? "N/A",
                LearnerName = r.LearnerName ?? "N/A",
                CourseName = r.CourseName ?? "N/A" // Map Course Title
            })
            .ToList();
    }
}
