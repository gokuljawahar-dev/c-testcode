namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class QuizReportRepository(
    LXPDbContext lXPDbContext,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccessor
) : IQuizReportRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

    public IEnumerable<QuizReportViewModel> GetQuizReports()
    {
        var quizReports = this
            ._lXPDbContext.Quizzes.Select(q => new
            {
                courseName = q.Course.Title,
                topicName = q.Topic.Name,
                quizName = q.NameOfQuiz,
                q.QuizId,
                q.PassMark,
                q.LearnerAttempts.First().LearnerId,
            })
            .Select(q => new QuizReportViewModel
            {
                CourseName = q.courseName,
                TopicName = q.topicName,
                QuizName = q.quizName,
                QuizId = q.QuizId,
                NoOfPassedUsers = this
                    ._lXPDbContext.LearnerAttempts.Where(attempt => attempt.QuizId == q.QuizId)
                    .GroupBy(attempt => attempt.LearnerId)
                    .Count(group => group.Max(attempt => attempt.Score) >= q.PassMark),
                NoOfFailedUsers = this
                    ._lXPDbContext.LearnerAttempts.Where(attempt => attempt.QuizId == q.QuizId)
                    .GroupBy(attempt => attempt.LearnerId)
                    .Count(group => group.Max(attempt => attempt.Score) < q.PassMark),
                AverageScore = this
                    ._lXPDbContext.LearnerAttempts.Where(attempt => attempt.QuizId == q.QuizId)
                    .GroupBy(attempt => attempt.LearnerId)
                    .Select(group => group.Max(attempt => attempt.Score))
                    .DefaultIfEmpty()
                    .Average()
            });
        ;

        return quizReports;
    }

    public IEnumerable<QuizScorelearnerViewModel> GetPassdLearnersList(Guid Quizid)
    {
        var quiz = this._lXPDbContext.Quizzes.Find(Quizid);
        var attempts = this
            ._lXPDbContext.LearnerAttempts.Where(e =>
                quiz!.QuizId.Equals(e.QuizId) && e.Score >= quiz.PassMark
            )
            .GroupBy(m => m.LearnerId)
            .Select(m => new QuizScorelearnerViewModel
            {
                LearnerId = m.Key,
                LearnerAttempts = m.Max(e => e.AttemptCount),
                LearnerName =
                    m.First().Learner.LearnerProfiles.First().FirstName
                    + " "
                    + m.First().Learner.LearnerProfiles.First().LastName,
                Score = m.Max(e => e.Score),
                TotalNoofQuizAttempts = (int)quiz!.AttemptsAllowed!,
                Profilephoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    m.First().Learner.LearnerProfiles.First().ProfilePhoto
                ),
                EmailId = m.First().Learner.Email,
            })
            .ToList();
        return attempts;
    }

    public IEnumerable<QuizScorelearnerViewModel> GetFailedLearnersList(Guid Quizid)
    {
        var quiz = this._lXPDbContext.Quizzes.Find(Quizid);
        var attempts = this
            ._lXPDbContext.LearnerAttempts.Where(e =>
                quiz!.QuizId.Equals(e.QuizId) && e.Score <= quiz.PassMark
            )
            .GroupBy(m => m.LearnerId)
            .Select(m => new QuizScorelearnerViewModel
            {
                LearnerId = m.Key,
                LearnerAttempts = m.Max(e => e.AttemptCount),
                LearnerName =
                    m.First().Learner.LearnerProfiles.First().FirstName
                    + " "
                    + m.First().Learner.LearnerProfiles.First().LastName,
                Score = m.Max(e => e.Score),
                TotalNoofQuizAttempts = (int)quiz!.AttemptsAllowed!,
                Profilephoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    m.First().Learner.LearnerProfiles.First().ProfilePhoto
                ),
                EmailId = m.First().Learner.Email,
            })
            .ToList();
        return attempts;
    }
}
