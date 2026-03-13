namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class LearnerDashboardRepository(LXPDbContext lXPDbContext) : ILearnerDashboardRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;

    public List<Enrollment> GetLearnerCompletedCount(Guid learnerId) =>
        this
            ._lXPDbContext.Enrollments.Where(e =>
                e.LearnerId == learnerId && e.CompletedStatus == 1
            )
            .ToList();

    public List<Enrollment> GetLearnerenrolledCourseCount(Guid learnerId) =>
        this._lXPDbContext.Enrollments.Where(e => e.LearnerId == learnerId).ToList();

    public List<Enrollment> GetLearnerDashboardInProgressCount(Guid learnerId) =>
        this
            ._lXPDbContext.Enrollments.Where(e =>
                e.LearnerId == learnerId && e.CompletedStatus == 0
            )
            .ToList();
}
