namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface ILearnerDashboardService
{
    public LearnerDashboardCourseCountViewModel GetLearnerDashboardDetails(Guid learnerId);
}
