namespace LXP.Api.Controllers;

using System.Net;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CourseTopicController(ICourseTopicServices courseTopicServices) : BaseController
{
    private readonly ICourseTopicServices _courseTopicServices = courseTopicServices;

    [HttpPost("/lxp/course/topic")]
    public async Task<IActionResult> AddCourseTopic(CourseTopicViewModel courseTopic)
    {
        var CreatedTopic = await this._courseTopicServices.AddCourseTopic(courseTopic);
        if (CreatedTopic != null)
        {
            return this.Ok(this.CreateInsertResponse(CreatedTopic));
        }
        else
        {
            return this.Ok(
                this.CreateFailureResponse(
                    MessageConstants.MsgAlreadyExists,
                    (int)HttpStatusCode.PreconditionFailed
                )
            );
        }
    }

    [HttpGet("/lxp/courses/{courseId}/topic")]
    public IActionResult GetAllCourseTopicByCourseId(string courseId)
    {
        var CourseTopic = this._courseTopicServices.GetAllTopicDetailsByCourseId(courseId);
        return this.Ok(this.CreateSuccessResponse(CourseTopic));
    }

    //    bool updatedStatus = await _courseTopicServices.UpdateCourseTopic(courseTopic);
    //            CreateSuccessResponse(
    //                null
    //            )
    //        );

    //            CreateSuccessResponse(
    //                _courseTopicServices.GetTopicDetailsByTopicId(courseTopic.TopicId)
    //            )
    //        );
    //        CreateFailureResponse(
    //            MessageConstants.MsgAlreadyExists,
    //            (int)HttpStatusCode.PreconditionFailed
    //        )
    //    );
    [HttpPut("/lxp/course/topic")]
    public IActionResult UpdateCourseTopic(CourseTopicUpdateModel courseTopic)
    {
        var updatedStatus = this._courseTopicServices.UpdateCourseTopic(courseTopic);
        if (updatedStatus)
        {
            return this.Ok(this.CreateSuccessResponse(null));
        }
        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgAlreadyExists,
                (int)HttpStatusCode.PreconditionFailed
            )
        );
    }

    [HttpDelete("/lxp/course/topic/{topicId}")]
    public async Task<IActionResult> DeleteCourseTopic(string topicId)
    {
        var deletedStatus = await this._courseTopicServices.SoftDeleteTopic(topicId);
        if (deletedStatus)
        {
            return this.Ok(this.CreateSuccessResponse());
        }
        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgNotDeleted,
                (int)HttpStatusCode.MethodNotAllowed
            )
        );
    }

    [HttpGet("/lxp/course/topic/{topicId}")]
    public async Task<IActionResult> GetAllCourseTopicByTopicId(string topicId)
    {
        var CourseTopic = await this._courseTopicServices.GetTopicDetailsByTopicId(topicId);
        return this.Ok(this.CreateSuccessResponse(CourseTopic));
    }

    [HttpGet("/lxp/course/{id}/topic")]
    public async Task<IActionResult> GetCourseTopicByCourseId(string id)
    {
        var CourseTopic = this._courseTopicServices.GetTopicDetails(id);
        return this.Ok(this.CreateSuccessResponse(CourseTopic));
    }
}
