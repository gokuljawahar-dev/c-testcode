namespace LXP.Api.Controllers;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

///<summary>
///Learner Video progress
///</summary>
[Route("api/[controller]")]
[ApiController]
public class LearnerProgressController(ILearnerProgressService Progress) : BaseController
{
    private readonly ILearnerProgressService _Progress = Progress;

    [HttpPost("/lxp/course/learner/learnerprogress")]
    public async Task<IActionResult> MaterialProgress(ProgressViewModel learnerProgress)
    {
        var result = await this._Progress.LearnerProgress(learnerProgress);
        return this.Ok(result);
    }

    [HttpGet("/lxp/course/learner/learnerprogress/watchtime")]
    public async Task<IActionResult> GetLearnerProgressByLearnerIdAndMaterialId(
        string LearnerId,
        string MaterialId
    ) =>
        this.Ok(
            this.CreateSuccessResponse(
                await this._Progress.GetLearnerProgressByLearnerIdAndMaterialId(
                    LearnerId,
                    MaterialId
                )
            )
        );


    [HttpGet("course-completion-percentage/{learnerId}/{enrollmentId}")]
    public async Task<IActionResult> GetCourseCompletionPercentage(
        Guid learnerId,
        Guid enrollmentId
    )
    {
        try
        {
            var (percentage, courseId) = await this._Progress.GetCourseCompletionAndCourseIdAsync(
                learnerId,
                enrollmentId
            );
            if (percentage.HasValue && courseId.HasValue)
            {
                return this.Ok(
                    new { CourseCompletionPercentage = percentage.Value, CourseId = courseId.Value }
                );
            }
            else
            {
                return this.NotFound(new { Message = "Enrollment not found." });
            }
        }
        catch (Exception ex)
        {
            return this.StatusCode(
                500,
                new
                {
                    Message = "An error occurred while fetching the course completion percentage and course ID.",
                    Details = ex.Message
                }
            );
        }
    }

    [HttpGet("{materialId}/progress/{learnerId}")]
    public async Task<ActionResult<double>> GetMaterialProgress(Guid materialId, Guid learnerId)
    {
        var progress = await this._Progress.CalculateMaterialProgressAsync(materialId, learnerId);
        return this.Ok(progress);
    }


}


//    try
//            new
//                Message = "Course completion percentage calculated and updated successfully."
//        );
//    catch (Exception ex)
//            500,
//            new
//                Message = "An error occurred while calculating the course completion.",
//                Details = ex.Message
//        );


//    try
//        // Call your learner progress service to calculate combined progress

//            CombinedProgress = combinedProgress,
//            CourseId = courseId
//    catch (Exception ex)
//        // Handle exceptions (e.g., invalid input, database errors)
