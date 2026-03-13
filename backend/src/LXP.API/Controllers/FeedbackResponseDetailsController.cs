namespace LXP.API.Controllers;


using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FeedbackResponseDetailsController(
    IFeedbackResponseDetailsService feedbackResponseDetailsService
) : ControllerBase
{
    private readonly IFeedbackResponseDetailsService _feedbackResponseDetailsService =
        feedbackResponseDetailsService;

    /// <summary>
    /// Retrieves feedback responses for a specific quiz by its ID.
    /// </summary>
    /// <param name="quizId">The unique identifier of the quiz.</param>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of feedback responses for the quiz.</response>
    /// <response code="404">Not found if no feedback responses exist for the provided quiz ID.</response>
    [HttpGet("quiz/{quizId}")]
    public IActionResult GetQuizFeedbackResponses(Guid quizId)
    {
        var responses = this._feedbackResponseDetailsService.GetQuizFeedbackResponses(quizId);
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves feedback responses for a specific topic by its ID.
    /// </summary>
    /// <param name="topicId">The unique identifier of the topic.</param>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of feedback responses for the topic.</response>
    /// <response code="404">Not found if no feedback responses exist for the provided topic ID.</response>
    [HttpGet("topic/{topicId}")]
    public IActionResult GetTopicFeedbackResponses(Guid topicId)
    {
        var responses = this._feedbackResponseDetailsService.GetTopicFeedbackResponses(topicId);
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves feedback responses for a specific quiz and learner by their IDs.
    /// </summary>
    /// <param name="quizId">The unique identifier of the quiz.</param>
    /// <param name="learnerId">The unique identifier of the learner.</param>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of feedback responses for the quiz by the learner.</response>
    /// <response code="404">Not found if no feedback responses exist for the provided quiz ID and learner ID.</response>
    [HttpGet("quiz/{quizId}/learner/{learnerId}")]
    public IActionResult GetQuizFeedbackResponsesByLearner(Guid quizId, Guid learnerId)
    {
        var responses = this._feedbackResponseDetailsService.GetQuizFeedbackResponsesByLearner(
            quizId,
            learnerId
        );
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves feedback responses for a specific topic and learner by their IDs.
    /// </summary>
    /// <param name="topicId">The unique identifier of the topic.</param>
    /// <param name="learnerId">The unique identifier of the learner.</param>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of feedback responses for the topic by the learner.</response>
    /// <response code="404">Not found if no feedback responses exist for the provided topic ID and learner ID.</response>
    [HttpGet("topic/{topicId}/learner/{learnerId}")]
    public IActionResult GetTopicFeedbackResponsesByLearner(Guid topicId, Guid learnerId)
    {
        var responses = this._feedbackResponseDetailsService.GetTopicFeedbackResponsesByLearner(
            topicId,
            learnerId
        );
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves all feedback responses for quizzes.
    /// </summary>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of all feedback responses for quizzes.</response>
    [HttpGet("quiz")]
    public IActionResult GetAllQuizFeedbackResponses()
    {
        var responses = this._feedbackResponseDetailsService.GetAllQuizFeedbackResponses();
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves all feedback responses for topics.
    /// </summary>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of all feedback responses for topics.</response>
    [HttpGet("topic")]
    public IActionResult GetAllTopicFeedbackResponses()
    {
        var responses = this._feedbackResponseDetailsService.GetAllTopicFeedbackResponses();
        return this.Ok(responses);
    }

    //course


    /// <summary>
    /// Retrieves feedback responses for a specific course by its ID.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of feedback responses for the course.</response>
    /// <response code="404">Not found if no feedback responses exist for the provided course ID.</response>
    [HttpGet("course/{courseId}")]
    public IActionResult GetCourseFeedbackResponses(Guid courseId)
    {
        var responses = this._feedbackResponseDetailsService.GetCourseFeedbackResponses(courseId);
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves feedback responses for a specific course and learner by their IDs.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="learnerId">The unique identifier of the learner.</param>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of feedback responses for the course by the learner.</response>
    /// <response code="404">Not found if no feedback responses exist for the provided course ID and learner ID.</response>
    [HttpGet("course/{courseId}/learner/{learnerId}")]
    public IActionResult GetCourseFeedbackResponsesByLearner(Guid courseId, Guid learnerId)
    {
        var responses = this._feedbackResponseDetailsService.GetCourseFeedbackResponsesByLearner(
            courseId,
            learnerId
        );
        return this.Ok(responses);
    }

    /// <summary>
    /// Retrieves all feedback responses for courses.
    /// </summary>
    /// <response code="200">Success on finding the feedback responses. The response body contains the list of all feedback responses for courses.</response>
    [HttpGet("course")]
    public IActionResult GetAllCourseFeedbackResponses()
    {
        var responses = this._feedbackResponseDetailsService.GetAllCourseFeedbackResponses();
        return this.Ok(responses);
    }
}
