namespace LXP.Api.Controllers;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentController(IEnrollmentService enrollmentService) : BaseController
{
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    [HttpPost("/lxp/enroll")]
    public async Task<IActionResult> Addenroll(EnrollmentViewModel enroll)
    {
        //validate model state
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var isEnrolled = await this._enrollmentService.Addenroll(enroll);

        if (isEnrolled)
        {
            return this.Ok(this.CreateSuccessResponse(null));
        }
        return this.Ok(this.CreateFailureResponse("AlreadyEnrolled", 400));
    }

    [HttpGet("/lxp/enroll/{learnerId}/course/topic")]
    public IActionResult GetCourseandTopicsByLearnerId(Guid learnerId)
    {
        var learner = this._enrollmentService.GetCourseandTopicsByLearnerId(learnerId);
        return this.Ok(this.CreateSuccessResponse(learner));
    }

    [HttpGet("lxp/enrollment/report")]
    public IActionResult GetAllEnrollemet()
    {
        var report = this._enrollmentService.GetEnrollmentsReport();
        return this.Ok(this.CreateSuccessResponse(report));
    }

    [HttpGet("lxp/enrollment/course/{courseId}")]
    public IActionResult GetEnrolledUsers(Guid courseId)
    {
        var enrolledusers = this._enrollmentService.GetEnrolledUsers(courseId);
        return this.Ok(this.CreateSuccessResponse(enrolledusers));
    }

    [HttpGet("lxp/enrollment/Inprogress/LearnerList")]
    public IActionResult GetInProgressLearnerList(Guid courseId)
    {
        var users = this._enrollmentService.GetEnrolledInprogressLearnerbyCourseId(courseId);
        return this.Ok(this.CreateSuccessResponse(users));
    }

    [HttpGet("lxp/enrollment/Completed/LearnerList")]
    public IActionResult GetCompletedLearnerList(Guid courseId)
    {
        var users = this._enrollmentService.GetEnrolledCompletedLearnerbyCourseId(courseId);
        return this.Ok(this.CreateSuccessResponse(users));
    }

    [HttpGet("/lxp/enroll/{learnerId}/course/{courseId}/topic")]
    public IActionResult GetCourseandTopicsByCourseId(Guid courseId, Guid learnerId)
    {
        var courses = this._enrollmentService.GetCourseandTopicsByCourseId(courseId, learnerId);
        return this.Ok(this.CreateSuccessResponse(courses));
    }

    [HttpDelete("lxp/enroll/delete/{enrollmentId}")]
    public async Task<IActionResult> DeleteEnrollment(Guid enrollmentId)
    {
        var enrollment = await this._enrollmentService.DeleteEnrollment(enrollmentId);

        if (enrollment)
        {
            return this.Ok(this.CreateSuccessResponse(enrollment));
        }
        return this.Ok(this.CreateFailureResponse("Enrollment Id is not found", 400));
    }

    [HttpPut("lxp/updateCourseStarted/{enrollmentId}")]
    public async Task UpdateCourseStarted(Guid enrollmentId) =>
        await this._enrollmentService.UpdateCourseStarted(enrollmentId);
}
