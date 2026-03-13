namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class CourseTopicRepository(
    LXPDbContext lXPDbContext,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor
) : ICourseTopicRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

    public async Task AddCourseTopic(Topic topic)
    {
        await this._lXPDbContext.Topics.AddAsync(topic);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public bool AnyTopicByTopicNameAndCourseId(string topicName, Guid courseId) =>
        this._lXPDbContext.Topics.Any(topic =>
            topic.Name == topicName && topic.CourseId == courseId && topic.IsActive
        );

    public object GetTopicDetails(string courseId)
    {
        var result =
            from course in this._lXPDbContext.Courses
            where course.CourseId == Guid.Parse(courseId)
            select new
            {
                course.CourseId,
                CourseTitle = course.Title,
                CourseIsActive = course.IsActive,
                Topics = (
                    from topic in this._lXPDbContext.Topics
                    where topic.CourseId == course.CourseId
                    select new
                    {
                        TopicName = topic.Name,
                        TopicDescription = topic.Description,
                        topic.TopicId,
                        TopicIsActive = topic.IsActive,
                        Rating = (
                            from tfq in this._lXPDbContext.TopicFeedbackQuestions
                            join fr in this._lXPDbContext.FeedbackResponses
                                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                            join fqo in this._lXPDbContext.FeedbackQuestionsOptions
                                on fr.OptionId equals fqo.FeedbackQuestionOptionId
                            where tfq.TopicId == topic.TopicId
                            select (decimal?)Convert.ToDecimal(fqo.OptionText)
                        ).Average() ?? 0,

                        Materials = (
                            from material in this._lXPDbContext.Materials
                            join materialType in this._lXPDbContext.MaterialTypes
                                on material.MaterialTypeId equals materialType.MaterialTypeId
                            where material.TopicId == topic.TopicId
                            select new
                            {
                                material.MaterialId,
                                MaterialName = material.Name,
                                MaterialType = materialType.Type,
                                MaterialDuration = material.Duration
                            }
                        ).ToList(),
                        FeddbackResponses = (
                            from c in this._lXPDbContext.Topics
                            join tfq in this._lXPDbContext.TopicFeedbackQuestions
                                on c.TopicId equals tfq.TopicId
                                into tfqGroup
                            from tfq in tfqGroup.DefaultIfEmpty()
                            join fr in this._lXPDbContext.FeedbackResponses
                                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                                into frGroup
                            from fr in frGroup.DefaultIfEmpty()
                            join lp in this._lXPDbContext.LearnerProfiles
                                on fr.LearnerId equals lp.LearnerId
                                into lpGroup
                            from lp in lpGroup.DefaultIfEmpty()
                            where c.TopicId == topic.TopicId
                            select new
                            {
                                Response = fr.Response ?? "NULL",
                                LearnerName = lp.FirstName ?? "NULL",
                            }
                        ).ToList()
                    }
                ).ToList()
            };

        return result;
    }

    public object GetAllTopicDetailsByCourseId(string courseId)
    {
        var result =
            from course in this._lXPDbContext.Courses
            where course.CourseId == Guid.Parse(courseId)
            select new
            {
                course.CourseId,
                CourseTitle = course.Title,
                CourseIsActive = course.IsActive,
                Topics = (
                    from topic in this._lXPDbContext.Topics
                    where topic.CourseId == course.CourseId && topic.IsActive
                    orderby topic.CreatedAt
                    select new
                    {
                        TopicName = topic.Name,
                        TopicDescription = topic.Description,
                        topic.TopicId,
                        TopicIsActive = topic.IsActive,
                        IsQuiz = this._lXPDbContext.Quizzes.Any(quizzes =>
                            quizzes.TopicId == topic.TopicId
                        ),
                        Rating = (
                            from tfq in this._lXPDbContext.TopicFeedbackQuestions
                            join fr in this._lXPDbContext.FeedbackResponses
                                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                            join fqo in this._lXPDbContext.FeedbackQuestionsOptions
                                on fr.OptionId equals fqo.FeedbackQuestionOptionId
                            where tfq.TopicId == topic.TopicId
                            select (decimal?)Convert.ToDecimal(fqo.OptionText)
                        ).Average() ?? 0,
                        Materials = (
                            from material in this._lXPDbContext.Materials
                            join materialType in this._lXPDbContext.MaterialTypes
                                on material.MaterialTypeId equals materialType.MaterialTypeId
                            where material.TopicId == topic.TopicId && material.IsActive
                            orderby material.CreatedAt
                            select new
                            {
                                material.MaterialId,
                                MaterialName = material.Name,
                                MaterialType = materialType.Type,
                                FilePath = string.Format(
                                    "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                                    this._contextAccessor.HttpContext.Request.Scheme,
                                    this._contextAccessor.HttpContext.Request.Host,
                                    this._contextAccessor.HttpContext.Request.PathBase,
                                    material.FilePath
                                ),

                                MaterialDuration = material.Duration
                            }
                        ).ToList(),
                        FeddbackResponses = (
                            from c in this._lXPDbContext.Topics
                            join tfq in this._lXPDbContext.TopicFeedbackQuestions
                                on c.TopicId equals tfq.TopicId
                                into tfqGroup
                            from tfq in tfqGroup.DefaultIfEmpty()
                            join fr in this._lXPDbContext.FeedbackResponses
                                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                                into frGroup
                            from fr in frGroup.DefaultIfEmpty()
                            where c.TopicId == topic.TopicId
                            select new { Response = fr.Response ?? "NULL" }
                        ).ToList()
                    }
                ).ToList()
            };

        return result;
    }

    public bool AnyTopicByTopicName(string topicName) =>
        this._lXPDbContext.Topics.Any(topic => topic.Name == topicName);

    public async Task<Topic> GetTopicByTopicId(Guid topicId) =>
        await this._lXPDbContext.Topics.FindAsync(topicId);

    public async Task<Topic> GetTopicDetailsByTopicNameAndCourse(string topicName, Guid courseId) =>
        await this._lXPDbContext.Topics.SingleAsync(topic =>
            topic.Name == topicName && topic.CourseId == courseId
        );

    public async Task<int> UpdateCourseTopic(Topic topic)
    {
        this._lXPDbContext.Topics.Update(topic);

        return await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task<List<Topic>> GetTopicsbycouresId(Guid courseId) =>
        await this._lXPDbContext.Topics.Where(topic => topic.CourseId == courseId).ToListAsync();

    public async Task<List<LearnerProgress>> GetTopicsbyLearnerId(Guid courseId, Guid LearnerId)
    {
        try
        {
            return await this
                ._lXPDbContext.LearnerProgresses.Where(learner =>
                    learner.CourseId == courseId && learner.LearnerId == LearnerId
                )
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public List<Topic> GetAllTopics() => this._lXPDbContext.Topics.ToList();
}
