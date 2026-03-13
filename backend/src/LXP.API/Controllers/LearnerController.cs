namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[Action]")]
[ApiController]
public class LearnerController(
    ILearnerServices learnerServices,
    IUserReportServices userReportServices
) : BaseController
{
    private readonly ILearnerServices _learnerServices = learnerServices;
    private readonly IUserReportServices _userReportServices = userReportServices;

    ///<summary>
    ///Getting All learners
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="404">Internal server Error</response>
    [HttpGet("/lxp/learner/getalllearnerdetails")]
    public IActionResult GetAllLearners()
    {
        var learners = this._learnerServices.GetLearners();

        return this.Ok(this.CreateSuccessResponse(learners));
    }

    ///<summary>
    ///Learner profile by learner id
    ///</summary>
    ///<response code="200">Success</response>
    [HttpGet("/lxp/learner/{learnerid}/learnerdetails")]
    public IActionResult GetAllLearnerDetailsByLearnerId(Guid learnerid)
    {
        var learner = this._learnerServices.GetAllLearnerDetailsByLearnerId(learnerid);
        return this.Ok(this.CreateSuccessResponse(learner));
    }

    ///<summary>
    ///Enrolled course details by learner id
    ///</summary>
    ///<response code="200">Success</response>

    [HttpGet("/lxp/learner/{learnerid}/entrolledcourse")]
    public IActionResult GetLearnerEntrolledcourseByLearnerId(Guid learnerid)
    {
        var learner = this._learnerServices.GetLearnerEnrolledcourseByLearnerId(learnerid);
        return this.Ok(this.CreateSuccessResponse(learner));
    }

    [HttpGet("/lxp/learnerReport")]
    public IActionResult GetLearnerReport()
    {
        var report = this._userReportServices.GetUserReport();
        return this.Ok(this.CreateSuccessResponse(report));
    }
}
