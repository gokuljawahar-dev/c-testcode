namespace LXP.Core.Services;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class LearnerDashboardService(ILearnerDashboardRepository learnerDashboardRepository)
    : ILearnerDashboardService
{
    private readonly ILearnerDashboardRepository _learnerDashboardRepository =
        learnerDashboardRepository;

    public LearnerDashboardCourseCountViewModel GetLearnerDashboardDetails(Guid learnerId)
    {
        var dashboarddetails = new LearnerDashboardCourseCountViewModel
        {
            CompletedCount = this
                ._learnerDashboardRepository.GetLearnerCompletedCount(learnerId)
                .Count,
            EnrolledCourseCount = this
                ._learnerDashboardRepository.GetLearnerenrolledCourseCount(learnerId)
                .Count,
            InProgressCount = this
                ._learnerDashboardRepository.GetLearnerDashboardInProgressCount(learnerId)
                .Count,
        };

        return dashboarddetails;
    }
}
