namespace LXP.Core.Services;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class DashboardService(IDashboardRepository dashboardRepository) : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository = dashboardRepository;

    public IEnumerable<DashboardLearnerViewModel> GetDashboardLearnerList() =>
        this._dashboardRepository.GetTotalLearners();

    IEnumerable<DashboardCourseViewModel> IDashboardService.GetDashboardCoursesList() =>
        this._dashboardRepository.GetTotalCourses();

    IEnumerable<DashboardEnrollmentViewModel> IDashboardService.GetDashboardEnrollmentList() =>
        this._dashboardRepository.GetTotalEnrollments();

    public Array GetMonthEnrollmentList(string year)
    {
        string[] month =
        [
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "June",
            "July",
            "Aug",
            "Sept",
            "Oct",
            "Nov",
            "Dec"
        ];
        var list = this._dashboardRepository.GetMonthWiseEnrollments(year).ToList();
        var query =
            from c in list
            orderby c.EnrollmentDate.Month
            group c by c.EnrollmentDate.Month into g
            select new { EnrollMonth = month[g.Key - 1], EnrollCount = g.Count() };
        var output = query.ToList();
        return output.ToArray();
    }

    public Array GetCourseCreatedList()
    {
        var list = this._dashboardRepository.GetCourseCreated().ToList();
        var query =
            from c in list
            group c by c.CreatedAt.Year into g
            select new { CreatedYear = g.Key, CourseCount = g.Count() };
        var output = query.ToList();
        return output.ToArray();
    }

    public string GetMostEnrolledCourse()
    {
        var course = this._dashboardRepository.GetMoreEnrolledCourse();
        return "hi";
    }

    //IEnumerable<DashboardEnrollmentViewModel> IDashboardService.GetEnrollments()


    public AdminDashboardViewModel GetAdminDashboardDetails()
    {
        var AdminDashboard = new AdminDashboardViewModel
        {
            NoOfLearners = this._dashboardRepository.GetNoOfLearners().Count,
            NoOfCourse = this._dashboardRepository.GetNoOfCourse().Count,
            NoOfActiveLearners = this._dashboardRepository.GetNoOfActiveLearners().Count,
            EnrollmentYears = this._dashboardRepository.GetEnrolledYears(),
            NoofInactiveLearners = this._dashboardRepository.GetNoOfInActiveLearners().Count,
            //HighestEnrolledCourse = _dashboardRepository.GetHighestEnrolledCourse(),
            //GetTopLearners = _dashboardRepository.GetTopLearners(),
            //GetTopFeedback = _dashboardRepository.GetFeedbackresponses(),
        };
        return AdminDashboard;
    }

    public IEnumerable<TopLearnersViewModel> GetTopLearner() =>
        this._dashboardRepository.GetTopLearner();

    public IEnumerable<HighestEnrolledCourseViewModel> GetHighestEnrolledCourse() =>
        this._dashboardRepository.GetHighestEnrolledCourse();

    public IEnumerable<RecentFeedbackViewModel> GetRecentfeedbackResponses() =>
        this._dashboardRepository.GetRecentfeedbackResponses();

    public Array GetCourseWiseEnrollmentsCount() =>
        this._dashboardRepository.GetEnrollmentCourse().ToArray();
}
