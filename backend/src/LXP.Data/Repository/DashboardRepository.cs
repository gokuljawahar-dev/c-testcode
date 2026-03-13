namespace LXP.Data.Repository;

using System.Data.Entity;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class DashboardRepository(
    LXPDbContext lXPDbContext,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor
) : IDashboardRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

    public IEnumerable<DashboardCourseViewModel> GetTotalCourses() =>
        this
            ._lXPDbContext.Courses.Select(x => new DashboardCourseViewModel
            {
                CourseId = x.CourseId,
                Title = x.Title,
            })
            .ToList();

    public IEnumerable<DashboardEnrollmentViewModel> GetTotalEnrollments() =>
        this
            ._lXPDbContext.Enrollments.Select(x => new DashboardEnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,
                CourseId = x.CourseId,
                LearnerId = x.LearnerId,
                EnrollmentDate = x.EnrollmentDate,
            })
            .ToList();

    public IEnumerable<DashboardLearnerViewModel> GetTotalLearners() =>
        this
            ._lXPDbContext.Learners.Select(x => new DashboardLearnerViewModel
            {
                LearnerId = x.LearnerId,
                Email = x.Email,
                Role = x.Role,
            })
            .Where(x => x.Role != "Admin")
            .ToList();

    public IEnumerable<DashboardEnrollmentViewModel> GetMonthWiseEnrollments(string year) =>

        this
            ._lXPDbContext.Enrollments.Select(x => new DashboardEnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,
                CourseId = x.CourseId,
                LearnerId = x.LearnerId,
                EnrollmentDate = x.EnrollmentDate,
            })
            .Where(x => x.EnrollmentDate.Year.ToString() == year)
            .ToList();

    public IEnumerable<DashboardCourseViewModel> GetCourseCreated() =>
        this
            ._lXPDbContext.Courses.Select(x => new DashboardCourseViewModel
            {
                CourseId = x.CourseId,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
            })
            .ToList();

    public IEnumerable<DashboardEnrollmentViewModel> GetMoreEnrolledCourse() =>
        this
            ._lXPDbContext.Enrollments.Select(x => new DashboardEnrollmentViewModel
            {
                EnrollmentId = x.EnrollmentId,
                CourseId = x.CourseId,
                LearnerId = x.LearnerId,
                EnrollmentDate = x.EnrollmentDate,
            })
            .ToList();

    public List<Learner> GetNoOfLearners() =>
        this._lXPDbContext.Learners.Where(Learner => Learner.Role != "Admin").ToList();

    public List<Course> GetNoOfCourse() => this._lXPDbContext.Courses.ToList();

    public List<Learner> GetNoOfActiveLearners()
    {
        var OneMonthAgo = DateTime.Now.AddMonths(-1);
        return
        [
            .. this._lXPDbContext.Learners.Where(Learner =>
                Learner.Role != "Admin" && Learner.UserLastLogin > OneMonthAgo
            ),
        ];
    }

    public List<string> GetTopLearners()
    {
        var topLearners = this
            ._lXPDbContext.Enrollments.GroupBy(e => e.LearnerId)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToList();
        var topLearnersWithNames = this
            ._lXPDbContext.LearnerProfiles.Where(p => topLearners.Contains(p.LearnerId))
            .Select(p => new { p.FirstName, p.LastName })
            .ToList()
            .Select(p => $"{p.FirstName} {p.LastName}")
            .ToList();

        return topLearnersWithNames;
    }

    public List<string> GetFeedbackresponses()
    {
        var feedbackResponses = this
            ._lXPDbContext.FeedbackResponses.OrderByDescending(e => e.GeneratedAt)
            .Where(p => p.Response != null)
            .Select(p => p.Response) // Select only the 'Response' property
            .Take(3)
            .ToList(); // Convert the result to a list of strings

        return feedbackResponses;
    }

    public IEnumerable<TopLearnersViewModel> GetTopLearner()
    {
        var topLearners = this
            ._lXPDbContext.Enrollments.GroupBy(e => e.LearnerId)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => new TopLearnersViewModel
            {
                Learnerid = g.Key,
                LearnerName =
                    g.First().Learner.LearnerProfiles.First().FirstName
                    + " "
                    + g.First().Learner.LearnerProfiles.First().LastName,
                ProfilePhoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    g.First().Learner.LearnerProfiles.First().ProfilePhoto
                )
            })
            .ToList();
        return topLearners;
    }

    public IEnumerable<HighestEnrolledCourseViewModel> GetHighestEnrolledCourse()
    {
        var HighestEnrolledCourses = this
            ._lXPDbContext.Enrollments.GroupBy(e => e.CourseId)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => new HighestEnrolledCourseViewModel
            {
                Courseid = g.Key,
                CourseName = g.First().Course.Title,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    g.First().Course.Thumbnail
                ),
                Learnerscount = g.Count(),
            })
            .ToList();
        return HighestEnrolledCourses;
    }

    public IEnumerable<RecentFeedbackViewModel> GetRecentfeedbackResponses()
    {
        var RecentfeedbackResponses = this
            ._lXPDbContext.FeedbackResponses.OrderByDescending(e => e.GeneratedAt)
            .Where(p => p.Response != null && p.TopicFeedbackQuestionId != null)
            .Take(3)
            .Select(g => new RecentFeedbackViewModel
            {
                Feedbackresponse = g.Response,
                Topicfeedbackquestions = g.TopicFeedbackQuestion!.Question,
                FeedbackresponseId = g.FeedbackResponseId,
                DateoftheResponse = (DateTime)g.GeneratedAt!,
                TopicName = g.TopicFeedbackQuestion!.Topic.Name,
                Coursename = g.TopicFeedbackQuestion.Topic.Course.Title,
                Learnerid = g.LearnerId,
                LearnerName =
                    g.Learner.LearnerProfiles.First().FirstName
                    + " "
                    + g.Learner.LearnerProfiles.First().LastName,
                Profilephoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    g.Learner.LearnerProfiles.First().ProfilePhoto
                )
            })
            .ToList();
        return RecentfeedbackResponses;
    }

    public List<string> GetEnrolledYears()
    {
        var years = this
            ._lXPDbContext.Enrollments.Select(p => p.EnrollmentDate.Year.ToString())
            .Distinct()
            .ToList();
        return years;
    }

    public List<Learner> GetNoOfInActiveLearners()
    {
        var OneMonthAgo = DateTime.Now.AddMonths(-1);
        return
        [
            .. this._lXPDbContext.Learners.Where(Learner =>
                Learner.Role != "Admin" && Learner.UserLastLogin < OneMonthAgo
            ),
        ];
    }

    public IEnumerable<CourseWiseViewModel> GetEnrollmentCourse()
    {
        var topLearners = this
            ._lXPDbContext.Enrollments.GroupBy(e => e.CourseId)
            .Select(g => new CourseWiseViewModel
            {
                CourseId = g.Key,
                Count = g.Count(),
                CourseName = g.First().Course.Title,
            })
            .ToList();
        return topLearners;
    }
}
