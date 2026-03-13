namespace LXP.Api.Controllers;

using LXP.Common.ViewModels.QuizFeedbackQuestionViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class QuizFeedbackController(IQuizFeedbackService quizFeedbackService) : BaseController
{
    private readonly IQuizFeedbackService _quizFeedbackService = quizFeedbackService;

    ///<summary>
    ///Add a new feedback question
    ///</summary>
    ///<param name="quizfeedbackquestion">Feedback question details</param>
    ///<response code="200">Feedback question added successfully</response>
    ///<response code="500">Internal server error</response>
    [HttpPost("AddFeedbackQuestion")]
    public IActionResult AddFeedbackQuestion(
        [FromBody] QuizfeedbackquestionViewModel quizfeedbackquestion
    )
    {
        var result = this._quizFeedbackService.AddFeedbackQuestion(
            quizfeedbackquestion,
            quizfeedbackquestion.Options
        );
        return this.Ok(this.CreateSuccessResponse(result));
    }

    ///<summary>
    ///Retrieve all feedback questions
    ///</summary>
    ///<response code="200">List of feedback questions</response>
    ///<response code="500">Internal server error</response>
    [HttpGet("GetAllFeedbackQuestions")]
    public IActionResult GetAllFeedbackQuestions()
    {
        var questions = this._quizFeedbackService.GetAllFeedbackQuestions();
        return this.Ok(this.CreateSuccessResponse(questions));
    }

    ///<summary>
    ///Get a feedback question by its ID
    ///</summary>
    ///<param name="quizFeedbackQuestionId">Feedback question ID</param>
    ///<response code="200">Feedback question details</response>
    ///<response code="404">Feedback question not found</response>
    ///<response code="500">Internal server error</response>
    [HttpGet("GetFeedbackQuestionById/{quizFeedbackQuestionId}")]
    public IActionResult GetFeedbackQuestionById(Guid quizFeedbackQuestionId)
    {
        var question = this._quizFeedbackService.GetFeedbackQuestionById(quizFeedbackQuestionId);
        if (question == null)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"Feedback question with ID {quizFeedbackQuestionId} not found.",
                    404
                )
            );
        }

        return this.Ok(this.CreateSuccessResponse(question));
    }

    ///<summary>
    ///Update a feedback question
    ///</summary>
    ///<param name="quizFeedbackQuestionId">Feedback question ID</param>
    ///<param name="quizfeedbackquestion">Updated feedback question details</param>
    ///<response code="204">Feedback question updated successfully</response>
    ///<response code="404">Feedback question not found</response>
    ///<response code="500">Internal server error</response>
    [HttpPut("UpdateFeedbackQuestion/{quizFeedbackQuestionId}")]
    public IActionResult UpdateFeedbackQuestion(
        Guid quizFeedbackQuestionId,
        [FromBody] QuizfeedbackquestionViewModel quizfeedbackquestion
    )
    {
        var existingQuestion = this._quizFeedbackService.GetFeedbackQuestionById(
            quizFeedbackQuestionId
        );
        if (existingQuestion == null)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"Feedback question with ID {quizFeedbackQuestionId} not found.",
                    404
                )
            );
        }

        var result = this._quizFeedbackService.UpdateFeedbackQuestion(
            quizFeedbackQuestionId,
            quizfeedbackquestion,
            quizfeedbackquestion.Options
        );
        if (!result)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"Failed to update feedback question with ID {quizFeedbackQuestionId}.",
                    500
                )
            );
        }

        return this.NoContent();
    }

    ///<summary>
    ///Delete a feedback question
    ///</summary>
    ///<param name="quizFeedbackQuestionId">Feedback question ID</param>
    ///<response code="204">Feedback question deleted successfully</response>
    ///<response code="404">Feedback question not found</response>
    ///<response code="500">Internal server error</response>
    [HttpDelete("DeleteFeedbackQuestion/{quizFeedbackQuestionId}")]
    public IActionResult DeleteFeedbackQuestion(Guid quizFeedbackQuestionId)
    {
        var existingQuestion = this._quizFeedbackService.GetFeedbackQuestionById(
            quizFeedbackQuestionId
        );
        if (existingQuestion == null)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"Feedback question with ID {quizFeedbackQuestionId} not found.",
                    404
                )
            );
        }

        var result = this._quizFeedbackService.DeleteFeedbackQuestion(quizFeedbackQuestionId);
        if (!result)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"Failed to delete feedback question with ID {quizFeedbackQuestionId}.",
                    500
                )
            );
        }

        return this.NoContent();
    }

    ///<summary>
    ///Get feedback questions by quiz ID
    ///</summary>
    ///<param name="quizId">Quiz ID</param>
    ///<response code="200">List of feedback questions</response>
    ///<response code="404">Feedback questions not found</response>
    ///<response code="500">Internal server error</response>
    [HttpGet("GetFeedbackQuestionsByQuizId/{quizId}")]
    public IActionResult GetFeedbackQuestionsByQuizId(Guid quizId)
    {
        var questions = this._quizFeedbackService.GetFeedbackQuestionsByQuizId(quizId);
        if (questions == null || questions.Count == 0)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"No feedback questions found for quiz ID {quizId}.",
                    404
                )
            );
        }
        return this.Ok(this.CreateSuccessResponse(questions));
    }

    ///<summary>
    ///Delete feedback questions by quiz ID
    ///</summary>
    ///<param name="quizId">Quiz ID</param>
    ///<response code="204">Feedback questions deleted successfully</response>
    ///<response code="404">Feedback questions not found</response>
    ///<response code="500">Internal server error</response>
    [HttpDelete("DeleteFeedbackQuestionsByQuizId/{quizId}")]
    public IActionResult DeleteFeedbackQuestionsByQuizId(Guid quizId)
    {
        var questions = this._quizFeedbackService.GetFeedbackQuestionsByQuizId(quizId);
        if (questions == null || questions.Count == 0)
        {
            return this.NotFound(
                this.CreateFailureResponse(
                    $"No feedback questions found for quiz ID {quizId}.",
                    404
                )
            );
        }

        var result = this._quizFeedbackService.DeleteFeedbackQuestionsByQuizId(quizId);
        if (!result)
        {
            return this.StatusCode(
                500,
                this.CreateFailureResponse(
                    $"Failed to delete feedback questions for quiz ID {quizId}.",
                    500
                )
            );
        }

        return this.NoContent();
    }
}
