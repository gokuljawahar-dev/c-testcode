namespace LXP.Api.Controllers;

using LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CourseFeedbackController(ICourseFeedbackService service) : BaseController
{
    private readonly ICourseFeedbackService _service = service;

    [HttpPost("question")]
    public IActionResult AddFeedbackQuestion(CourseFeedbackQuestionViewModel question)
    {
        if (question == null)
        {
            return this.BadRequest(this.CreateFailureResponse("Question object is null", 400));
        }

        var options = question.Options ?? []; // Ensure options are not null
        var questionId = this._service.AddFeedbackQuestion(question, options);

        if (questionId != Guid.Empty)
        {
            return this.Ok(this.CreateSuccessResponse("Question added successfully"));
        }

        return this.StatusCode(500, this.CreateFailureResponse("Failed to add question", 500));
    }

    [HttpGet]
    public IActionResult GetAllFeedbackQuestions()
    {
        var questions = this._service.GetAllFeedbackQuestions();
        return this.Ok(this.CreateSuccessResponse(questions));
    }

    [HttpGet("{courseFeedbackQuestionId}")]
    public IActionResult GetFeedbackQuestionById(Guid courseFeedbackQuestionId)
    {
        var question = this._service.GetFeedbackQuestionById(courseFeedbackQuestionId);
        if (question == null)
        {
            return this.NotFound(this.CreateFailureResponse("Feedback question not found", 404));
        }

        return this.Ok(this.CreateSuccessResponse(question));
    }

    [HttpPut("{courseFeedbackQuestionId}")]
    public IActionResult UpdateFeedbackQuestion(
        Guid courseFeedbackQuestionId,
        CourseFeedbackQuestionViewModel question
    )
    {
        var existingQuestion = this._service.GetFeedbackQuestionById(courseFeedbackQuestionId);
        if (existingQuestion == null)
        {
            return this.NotFound(this.CreateFailureResponse("Feedback question not found", 404));
        }

        var options = question.Options ?? []; // Ensure options are not null
        var result = this._service.UpdateFeedbackQuestion(
            courseFeedbackQuestionId,
            question,
            options
        );

        if (result)
        {
            return this.Ok(this.CreateSuccessResponse("Feedback question updated successfully"));
        }

        return this.StatusCode(
            500,
            this.CreateFailureResponse("Failed to update feedback question", 500)
        );
    }

    [HttpDelete("{courseFeedbackQuestionId}")]
    public IActionResult DeleteFeedbackQuestion(Guid courseFeedbackQuestionId)
    {
        var existingQuestion = this._service.GetFeedbackQuestionById(courseFeedbackQuestionId);
        if (existingQuestion == null)
        {
            return this.NotFound(this.CreateFailureResponse("Feedback question not found", 404));
        }

        var result = this._service.DeleteFeedbackQuestion(courseFeedbackQuestionId);

        if (result)
        {
            return this.Ok(this.CreateSuccessResponse("Feedback question deleted successfully"));
        }

        return this.StatusCode(
            500,
            this.CreateFailureResponse("Failed to delete feedback question", 500)
        );
    }

    [HttpGet("course/{courseId}")]
    public IActionResult GetFeedbackQuestionsByCourseId(Guid courseId)
    {
        var questions = this._service.GetFeedbackQuestionsByCourseId(courseId);
        if (questions == null || questions.Count == 0)
        {
            return this.NotFound(
                this.CreateFailureResponse("No questions found for the given course", 404)
            );
        }

        return this.Ok(this.CreateSuccessResponse(questions));
    }
}
