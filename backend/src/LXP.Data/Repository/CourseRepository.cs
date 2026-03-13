namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class CourseRepository(
    LXPDbContext lXPDbContext,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor
) : ICourseRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

    public Course GetCourseDetailsByCourseName(string courseName) =>
        this
            ._lXPDbContext.Courses.Include(course => course.Level)
            .Include(course => course.Category)
            .FirstOrDefault(course => course.Title == courseName);

    public void AddCourse(Course course)
    {
        this._lXPDbContext.Courses.Add(course);
        this._lXPDbContext.SaveChanges();
    }

    public bool AnyCourseByCourseTitle(string courseTitle) =>
        this._lXPDbContext.Courses.Any(course => course.Title == courseTitle);

    public Course GetCourseDetailsByCourseId(Guid CourseId) =>
        this
            ._lXPDbContext.Courses.Include(course => course.Level)
            .Include(course => course.Category)
            .FirstOrDefault(course => course.CourseId == CourseId);

    public Course FindCourseid(Guid courseid) => this._lXPDbContext.Courses.Find(courseid);

    public Enrollment FindEntrollmentcourse(Guid Courseid) =>
        this._lXPDbContext.Enrollments.FirstOrDefault(Course => Course.CourseId == Courseid);

    public async Task Deletecourse(Course course)
    {
        this._lXPDbContext.Courses.Remove(course);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task Changecoursestatus(Course course)
    {
        this._lXPDbContext.Courses.Update(course);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task Updatecourse(Course course)
    {
        this._lXPDbContext.Courses.Update(course);
        await this._lXPDbContext.SaveChangesAsync();
    }

    //     // Get the course ratings
    //         .Courses.Select(c => new CourseDetailsViewModel
    //             CourseId = c.CourseId,
    //             Status = c.IsAvailable,
    //             Title = c.Title,
    //             Level = c.Level.Level,
    //             Category = c.Category.Category,
    //             Duration = c.Duration,
    //             Description = c.Description,
    //             CreatedAt = c.CreatedAt,
    //             CategoryId = c.CategoryId,
    //             LevelId = c.LevelId,
    //             Thumbnailimage = string.Format(
    //                 "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
    //                 this._contextAccessor.HttpContext.Request.Scheme,
    //                 this._contextAccessor.HttpContext.Request.Host,
    //                 this._contextAccessor.HttpContext.Request.PathBase,
    //                 c.Thumbnail
    //             ),
    //             ModifiedAt = c.ModifiedAt.ToString(),
    //             AverageRating = courseRatings.TryGetValue(c.CourseId, out var value) ? value.Rating
    //                 : 0
    //         .ToList();
    public IEnumerable<CourseDetailsViewModel> GetAllCourse()
    {
        // Get the course ratings
        var courseRatings = this.GetCourseRating().ToDictionary(cr => cr.CourseId);
        return this
            ._lXPDbContext.Courses.AsEnumerable() // Convert to in-memory collection
            .Select(c => new CourseDetailsViewModel
            {
                CourseId = c.CourseId,
                Status = c.IsAvailable,
                Title = c.Title,
                Level = c.Level.Level,
                Category = c.Category.Category,
                Duration = c.Duration,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                CategoryId = c.CategoryId,
                LevelId = c.LevelId,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    c.Thumbnail
                ),
                ModifiedAt = c.ModifiedAt.ToString(),
                AverageRating = courseRatings.TryGetValue(c.CourseId, out var value)
                    ? value.Rating
                    : 0
            })
            .ToList();
    }

    //         .Courses.OrderByDescending(c => c.CreatedAt)
    //         .Select(c => new CourseDetailsViewModel
    //             CourseId = c.CourseId,
    //             Title = c.Title,
    //             Level = c.Level.Level,
    //             Category = c.Category.Category,
    //             Duration = c.Duration,
    //             Thumbnailimage = string.Format(
    //                 "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
    //                 this._contextAccessor.HttpContext.Request.Scheme,
    //                 this._contextAccessor.HttpContext.Request.Host,
    //                 this._contextAccessor.HttpContext.Request.PathBase,
    //                 c.Thumbnail
    //             ),
    //             CreatedAt = c.CreatedAt,
    //             AverageRating = courseRating.TryGetValue(c.CourseId, out var value) ? value.Rating
    //                 : 0
    //         .Take(9)
    //         .ToList();

    public IEnumerable<CourseDetailsViewModel> GetLimitedCourse()
    {
        var courseRating = this.GetCourseRating().ToDictionary(cr => cr.CourseId);
        return this
            ._lXPDbContext.Courses.OrderByDescending(c => c.CreatedAt)
            .AsEnumerable() // Convert to in-memory collection
            .Select(c => new CourseDetailsViewModel
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Level = c.Level.Level,
                Category = c.Category.Category,
                Duration = c.Duration,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    c.Thumbnail
                ),
                CreatedAt = c.CreatedAt,
                AverageRating = courseRating.TryGetValue(c.CourseId, out var value)
                    ? value.Rating
                    : 0
            })
            .Take(9)
            .ToList();
    }

    public IEnumerable<CourseListViewModel> GetAllCourseDetails() =>
        this
            ._lXPDbContext.Courses.Select(c => new CourseListViewModel
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description,
                Level = c.Level.Level,
                Category = c.Category.Category,
                Duration = c.Duration,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    c.Thumbnail
                ),
            })
            .ToList();

    public async Task<dynamic> GetAllCourseDetailsByLearnerId(Guid learnerId)
    {
        var query =
            from course in this._lXPDbContext.Courses
            join enrollment in this._lXPDbContext.Enrollments
                on new { course.CourseId, LearnerId = learnerId } equals new
                {
                    enrollment.CourseId,
                    enrollment.LearnerId
                }
                into enrollments
            from enrollment in enrollments.DefaultIfEmpty()
            where course.IsAvailable && course.IsActive
            orderby course.CreatedAt descending, course.CourseId, enrollment.EnrollmentId // Added 'descending' for CreatedAt
            select new
            {
                course.CourseId,
                course.Category.Category,
                course.Level.Level,
                course.Title,
                course.Description,
                course.Duration,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    course.Thumbnail
                ),
                course.CreatedBy,
                course.CreatedAt,
                IsActive = true,
                IsAvailable = true,
                course.ModifiedAt,
                course.ModifiedBy,
                EnrollStatus = enrollment.EnrollStatus != null && enrollment.EnrollStatus
            };
        return query.ToList();
    }

    public IEnumerable<CourseRatingViewModel> GetCourseRating()
    {
        var queryrating =
            from c in this._lXPDbContext.Courses
            join cfq in this._lXPDbContext.CourseFeedbackQuestions
                on c.CourseId equals cfq.CourseId
                into cfqGroup
            from cfq in cfqGroup.DefaultIfEmpty()
            join fr in this._lXPDbContext.FeedbackResponses
                on cfq.CourseFeedbackQuestionId equals fr.CourseFeedbackQuestionId
                into frGroup
            from fr in frGroup.DefaultIfEmpty()
            join fqo in this._lXPDbContext.FeedbackQuestionsOptions
                on fr.OptionId equals fqo.FeedbackQuestionOptionId
                into fqoGroup
            from fqo in fqoGroup.DefaultIfEmpty()
            group new { c, fqo } by new { c.CourseId, c.Title } into g
            select new CourseRatingViewModel
            {
                CourseId = g.Key.CourseId,
                Title = g.Key.Title,
                Rating = g.Average(x => (decimal?)Convert.ToDecimal(x.fqo.OptionText)) ?? 0
            };

        return queryrating.ToList();
    }

    public IEnumerable<TopicRatingViewModel> GetTopicRating()
    {
        var queryTopicRating =
            from t in this._lXPDbContext.Topics
            join tfq in this._lXPDbContext.TopicFeedbackQuestions
                on t.TopicId equals tfq.TopicId
                into tfqGroup
            from tfq in tfqGroup.DefaultIfEmpty()
            join fr in this._lXPDbContext.FeedbackResponses
                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                into frGroup
            from fr in frGroup.DefaultIfEmpty()
            join fqo in this._lXPDbContext.FeedbackQuestionsOptions
                on fr.OptionId equals fqo.FeedbackQuestionOptionId
                into fqoGroup
            from fqo in fqoGroup.DefaultIfEmpty()
            group new { t, fqo } by new { t.TopicId, t.Name } into g
            select new TopicRatingViewModel
            {
                TopicId = g.Key.TopicId,
                Name = g.Key.Name,
                Rating = g.Average(x => (decimal?)Convert.ToDecimal(x.fqo.OptionText)) ?? 0
            };

        return queryTopicRating.ToList();
    }
}
