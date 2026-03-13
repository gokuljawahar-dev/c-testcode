namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface ILearnerDashboardRepository
{
    public List<Enrollment> GetLearnerenrolledCourseCount(Guid learnerId);
    public List<Enrollment> GetLearnerCompletedCount(Guid learnerId);
    public List<Enrollment> GetLearnerDashboardInProgressCount(Guid learnerId);
}
