namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;

public class LearnerRepository(
    LXPDbContext context,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor
) : ILearnerRepository
{
    private readonly LXPDbContext _lXPDbContext = context;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

    public void AddLearner(Learner learner)
    {
        this._lXPDbContext.Learners.Add(learner);

        this._lXPDbContext.SaveChanges();
    }

    public async Task<bool> AnyLearnerByEmail(string email) =>
        this._lXPDbContext.Learners.Any(l => l.Email == email);

    public Learner GetLearnerByLearnerEmail(string email) =>
        this._lXPDbContext.Learners.FirstOrDefault(learner => learner.Email == email);

    public async Task<List<Learner>> GetAllLearner() => this._lXPDbContext.Learners.ToList();

    public Learner GetLearnerDetailsByLearnerId(Guid LearnerId) =>
        this._lXPDbContext.Learners.Find(LearnerId);

    public async Task UpdateLearner(Learner learner)
    {
        this._lXPDbContext.Learners.Update(learner);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public IEnumerable<AllLearnersViewModel> GetLearners() =>
        this
            ._lXPDbContext.LearnerProfiles.Select(c => new AllLearnersViewModel
            {
                LearnerID = c.LearnerId,
                LearnerName = c.FirstName + " " + c.LastName,
                Email = c.Learner.Email,
                LastLogin = c.Learner.UserLastLogin,
                LearnerStatus = c.Learner.UserLastLogin > DateTime.Now.AddMonths(-1),
            })
            .ToList();

    public object GetAllLearnerDetailsByLearnerId(Guid learnerId)
    {
        var result =
            from learner in this._lXPDbContext.Learners
            where learner.LearnerId == learnerId
            select new
            {
                LearnerEmail = learner.Email,
                LearnerLastlogin = learner.UserLastLogin,
                LearnerFirstName = learner.LearnerProfiles.First().FirstName,
                LearnerLastName = learner.LearnerProfiles.First().LastName,
                LearnerDob = learner.LearnerProfiles.First().Dob,
                LearnerGender = learner.LearnerProfiles.First().Gender,
                LearnerContactNumber = learner.LearnerProfiles.First().ContactNumber,
                LearnerStream = learner.LearnerProfiles.First().Stream,
                Learnerprofile = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext!.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    learner.LearnerProfiles.First().ProfilePhoto
                )
            };
        return result;
    }

    //GetLearnerEntrolledcourseByLearnerId

    public object GetLearnerEnrolledcourseByLearnerId(Guid learnerId)
    {
        var result =
            from enrollment in this._lXPDbContext.Enrollments
            where enrollment.LearnerId == learnerId
            orderby enrollment.EnrollmentDate descending
            select new
            {
                Enrollmentid = enrollment.EnrollmentId,
                Enrolledcourse = enrollment.Course.Title,
                EnrolledCourseCategory = enrollment.Course.Category.Category,
                EnrolledCourselevels = enrollment.Course.Level.Level,
                Enrollmentdate = enrollment.EnrollmentDate,
                CourseImage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    enrollment.Course.Thumbnail
                ),
                Status = enrollment.CompletedStatus,
            };
        return result;
    }

    public IDbContextTransaction BeginTransaction() =>
        this._lXPDbContext.Database.BeginTransaction();
}
