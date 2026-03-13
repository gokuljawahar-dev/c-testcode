namespace LXP.API.Controllers;

using FluentValidation;
using LXP.Common.ViewModels.FeedbackResponseViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Manages feedback response operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FeedbackResponseController"/> class.
/// </remarks>
/// <param name="feedbackResponseService">The feedback response service.</param>
[ApiController]
[Route("api/[controller]")]
public class FeedbackResponseController(IFeedbackResponseService feedbackResponseService)
    : ControllerBase
{
    private readonly IFeedbackResponseService _feedbackResponseService = feedbackResponseService;

    /// <summary>
    /// Adds new quiz feedback responses.
    /// </summary>
    /// <param name="feedbackResponses">The list of quiz feedback response models.</param>
    /// <returns>A response indicating the result of the feedback submission.</returns>
    /// <response code="201">Quiz feedback responses added successfully.</response>
    /// <response code="400">Bad request due to invalid input.</response>
    [HttpPost("AddQuizFeedbackResponses")]
    public IActionResult AddQuizFeedbackResponses(
        [FromBody] IEnumerable<QuizFeedbackResponseViewModel> feedbackResponses
    )
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            this._feedbackResponseService.SubmitFeedbackResponses(feedbackResponses);
            return this.Ok(new { Message = "Quiz feedback responses added successfully." });
        }
        catch (ValidationException ex)
        {
            return this.BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return this.BadRequest(new { ex.Message });
        }
    }

    /// <summary>
    /// Adds new topic feedback responses.
    /// </summary>
    /// <param name="feedbackResponses">The list of topic feedback response models.</param>
    /// <returns>A response indicating the result of the feedback submission.</returns>
    /// <response code="201">Topic feedback responses added successfully.</response>
    /// <response code="400">Bad request due to invalid input.</response>
    [HttpPost("AddTopicFeedbackResponses")]
    public IActionResult AddTopicFeedbackResponses(
        [FromBody] IEnumerable<TopicFeedbackResponseViewModel> feedbackResponses
    )
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            this._feedbackResponseService.SubmitFeedbackResponses(feedbackResponses);
            return this.Ok(new { Message = "Topic feedback responses added successfully." });
        }
        catch (ValidationException ex)
        {
            return this.BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return this.BadRequest(new { ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the feedback status of a quiz for a specific learner.
    /// </summary>
    /// <param name="learnerId">The ID of the learner.</param>
    /// <param name="quizId">The ID of the quiz.</param>
    /// <returns>A response containing the quiz feedback status for the learner.</returns>
    /// <response code="200">Quiz feedback status retrieved successfully.</response>
    /// <response code="400">Bad request due to an exception during the operation.</response>


    [HttpGet("QuizFeedbackStatus")]
    public IActionResult GetQuizFeedbackStatus(Guid learnerId, Guid quizId)
    {
        try
        {
            var status = this._feedbackResponseService.GetQuizFeedbackStatus(learnerId, quizId);
            return this.Ok(status);
        }
        catch (Exception ex)
        {
            return this.BadRequest(new { ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the feedback status of a topic for a specific learner.
    /// </summary>
    /// <param name="learnerId">The ID of the learner.</param>
    /// <param name="topicId">The ID of the topic.</param>
    /// <returns>A response containing the topic feedback status for the learner.</returns>
    /// <response code="200">Topic feedback status retrieved successfully.</response>
    /// <response code="400">Bad request due to an exception during the operation.</response>



    [HttpGet("TopicFeedbackStatus")]
    public IActionResult GetTopicFeedbackStatus(Guid learnerId, Guid topicId)
    {
        try
        {
            var status = this._feedbackResponseService.GetTopicFeedbackStatus(learnerId, topicId);
            return this.Ok(status);
        }
        catch (Exception ex)
        {
            return this.BadRequest(new { ex.Message });
        }
    }

    [HttpPost("SubmitCourseFeedbackResponses")]
    public IActionResult SubmitCourseFeedbackResponses(
        [FromBody] IEnumerable<CourseFeedbackResponseViewModel> feedbackResponses
    )
    {
        try
        {
            this._feedbackResponseService.SubmitFeedbackResponses(feedbackResponses);
            return this.Ok(new { Message = "Feedback responses submitted successfully." });
        }
        catch (Exception ex)
        {
            return this.BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("GetCourseFeedbackStatus/{learnerId}/{courseId}")]
    public IActionResult GetCourseFeedbackStatus(Guid learnerId, Guid courseId)
    {
        try
        {
            var status = this._feedbackResponseService.GetCourseFeedbackStatus(learnerId, courseId);
            return this.Ok(status);
        }
        catch (Exception ex)
        {
            return this.BadRequest(new { Error = ex.Message });
        }
    }
}
