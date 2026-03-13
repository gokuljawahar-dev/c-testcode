namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

///<summary>
/// Details required for Dashboard
///</summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class DashboardController(IDashboardService dashboardService) : BaseController
{
    private readonly IDashboardService _dashboardService = dashboardService;

    ///<summary>
    ///getting total number of enrollments according to month
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="500">Internal server Error</response>
    [HttpGet]
    public ActionResult GetEnrollmentMonth(string year)
    {
        var total_month_wise = this._dashboardService.GetMonthEnrollmentList(year);
        return this.Ok(this.CreateSuccessResponse(total_month_wise));
    }

    ///<summary>
    ///getting total number of course creation according to year
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="500">Internal server Error</response>
    [HttpGet]
    public ActionResult GetCourseCreated()
    {
        var total_course_created = this._dashboardService.GetCourseCreatedList();
        return this.Ok(this.CreateSuccessResponse(total_course_created));
    }

    ///<summary>
    ///getting total number of course creation according to year
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="500">Internal server Error</response>


    [HttpGet("/lxp/admin/DashboardDetails")]
    public IActionResult AdminDashboard()
    {
        var admin = this._dashboardService.GetAdminDashboardDetails();
        return this.Ok(this.CreateSuccessResponse(admin));
    }

    [HttpGet("/lxp/admin/GetTopLearners")]
    public IActionResult GetTopLearner()
    {
        var GetTopLearners = this._dashboardService.GetTopLearner();
        return this.Ok(this.CreateSuccessResponse(GetTopLearners));
    }

    [HttpGet("/lxp/admin/GetHighestEnrolledCourse")]
    public IActionResult GetHighestEnrolledCourse()
    {
        var GetHighestEnrolledCourse = this._dashboardService.GetHighestEnrolledCourse();
        return this.Ok(this.CreateSuccessResponse(GetHighestEnrolledCourse));
    }

    [HttpGet("/lxp/admin/GetRecentfeedbackResponses")]
    public IActionResult GetRecentfeedbackResponses()
    {
        var GetRecentfeedbackResponses = this._dashboardService.GetRecentfeedbackResponses();
        return this.Ok(this.CreateSuccessResponse(GetRecentfeedbackResponses));
    }

    [HttpGet("/lxp/admin/GetCourseWiseEnrollmentsCount")]
    public IActionResult GetCourseWiseEnrollmentsCount()
    {
        var GetCourseWiseEnrollmentsCount = this._dashboardService.GetCourseWiseEnrollmentsCount();
        return this.Ok(this.CreateSuccessResponse(GetCourseWiseEnrollmentsCount));
    }
}
