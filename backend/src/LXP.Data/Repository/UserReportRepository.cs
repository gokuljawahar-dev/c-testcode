namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;

public class UserReportRepository(LXPDbContext lXPDbContext) : IUserReportRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;

    public IEnumerable<UserReportViewModel> GetUserReport()
    {
        var query = this
            ._lXPDbContext.Learners.Where(e => e.Role != "Admin")
            .GroupBy(e => e.LearnerId)
            .Select(grouped => new UserReportViewModel
            {
                UserName =
                    $"{this._lXPDbContext.LearnerProfiles.Where(x => x.LearnerId.Equals(grouped.Key)).First().FirstName} {this._lXPDbContext.LearnerProfiles.Where(x => x.LearnerId.Equals(grouped.Key)).First().LastName}",
                LearnerId = grouped.Key.ToString(),
                EnrolledCourse = this
                    ._lXPDbContext.Enrollments.Where(e => e.LearnerId.Equals(grouped.Key))
                    .Count(),
                CompletedCourse = this
                    ._lXPDbContext.Enrollments.Where(e => e.LearnerId.Equals(grouped.Key))
                    .Count(x => x.CompletedStatus == 1),
                LastLogin = this
                    ._lXPDbContext.Learners.Where(x => x.LearnerId.Equals(grouped.Key))
                    .First()
                    .UserLastLogin
            });

        var userReports = query.ToList();
        return userReports;
    }
}
