namespace LXP.Api.Controllers;

using System.Net;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CourseController(ICourseServices courseServices) : BaseController
{
    private readonly ICourseServices _courseServices = courseServices;

    [HttpPost("/lxp/course")]
    public IActionResult AddCourseDetails(CourseViewModel course)
    {
        // Validate model state
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var CreatedCourse = this._courseServices.AddCourse(course);
        if (CreatedCourse != null)
        {
            return this.Ok(this.CreateInsertResponse(CreatedCourse));
        }
        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgAlreadyExists,
                (int)HttpStatusCode.PreconditionFailed
            )
        );
    }

    [HttpGet("/lxp/course/{id}")]
    public async Task<IActionResult> GetCourseDetailsByCourseId(string id)
    {
        var course = await this._courseServices.GetCourseDetailsByCourseId(id);
        return this.Ok(this.CreateSuccessResponse(course));
    }

    /////<summary>
    /////Getting all Course name and its id
    /////</summary>
    /////<response code="200">Success</response>
    /////<response code="404">Internal server Error</response>

    ///<summary>
    ///Update the course
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="405">Internal server Error</response>
    [HttpPut("lxp/courseupdate")]
    public async Task<IActionResult> Updatecourse([FromForm] CourseUpdateModel course)
    {
        var updatecourse = await this._courseServices.Updatecourse(course);

        if (updatecourse)
        {
            return this.Ok(this.CreateSuccessResponse(updatecourse));
        }

        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgNotUpdated,
                (int)HttpStatusCode.MethodNotAllowed
            )
        );
    }

    ///<summary>
    ///Delete the course
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="405">Internal server Error</response>
    [HttpDelete("/lxp/coursedelete/{id}")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var course = await this._courseServices.Deletecourse(id);

        if (course)
        {
            return this.Ok(this.CreateSuccessResponseForDelete(course));
        }
        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgCourseNotDeleted,
                (int)HttpStatusCode.MethodNotAllowed
            )
        );
    }

    ///<summary>
    ///Update the course status
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="405">Internal server Error</response>

    [HttpPut("/lxp/coursestatus")]
    public async Task<IActionResult> Coursestatus(Coursestatus CourseStatus)
    {
        var Coursestatus = await this._courseServices.Changecoursestatus(CourseStatus);

        if (Coursestatus)
        {
            return this.Ok(this.CreateSuccessResponse(Coursestatus));
        }
        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgNotUpdated,
                (int)HttpStatusCode.MethodNotAllowed
            )
        );
    }

    [HttpGet("lxp/GetAllCourse")]
    public IActionResult GetAllCourse()
    {
        var courses = this._courseServices.GetAllCourse();
        return this.Ok(this.CreateSuccessResponse(courses));
    }

    [HttpGet("lxp/Getninecourse")]
    public IActionResult GetLimitedCourse()
    {
        var course = this._courseServices.GetLimitedCourse();
        return this.Ok(this.CreateSuccessResponse(course));
    }

    ///<summary>
    ///Fetch all the course
    ///</summary>

    [HttpGet("/lxp/view/course")]
    public IActionResult GetAllCourseDetails()
    {
        var course = this._courseServices.GetAllCourseDetails();
        return this.Ok(this.CreateSuccessResponse(course));
    }


    [HttpGet("/lxp/view/Getallcoursebylearnerid/{learnerId}")]
    public async Task<IActionResult> GetAllCourseDetailsByLearnerId(string learnerId)
    {
        var Courses = this._courseServices.GetAllCourseDetailsByLearnerId(learnerId);
        return this.Ok(this.CreateSuccessResponse(Courses));
    }
}
