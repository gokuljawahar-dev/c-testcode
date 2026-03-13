namespace LXP.Api.Controllers;

using System.ComponentModel.DataAnnotations;
using System.Net;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base controller providing common functionalities and responses for API controllers.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Creates a success response with optional data.
    /// </summary>
    [NonAction]
    public APIResponse CreateSuccessResponse(dynamic result = null) =>
        new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = MessageConstants.MsgSuccess,
            Data = result
        };

    /// <summary>
    /// Creates a success response with optional data.
    /// </summary>
    [NonAction]
    public APIResponse CreateSuccessResponseForDelete(dynamic result = null) =>
        new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = MessageConstants.DeleteCourseSucessMessage,
            Data = result
        };

    /// <summary>
    /// Creates a failure response with specified message and status code.
    /// </summary>
    [NonAction]
    public APIResponse CreateFailureResponse(string message, int statusCode) =>
        new()
        {
            StatusCode = statusCode,
            Message = message,
            Data = null
        };

    /// <summary>
    /// Creates a response for successful data insertion with optional data.
    /// </summary>
    [NonAction]
    public APIResponse CreateInsertResponse(dynamic result) =>
        new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = MessageConstants.MsgCreated,
            Data = result
        };

    /// <summary>
    /// Creates a response for successful request with no content and optional data.
    /// </summary>
    [NonAction]
    public APIResponse CreateNoContentResponse(dynamic result) =>
        new()
        {
            StatusCode = (int)HttpStatusCode.NoContent,
            Message = MessageConstants.MsgNoContent,
            Data = result
        };

    /// <summary>
    /// Validates the given model and returns appropriate response if validation fails.
    /// </summary>
    [NonAction]
    public IActionResult ValidateModel(object model)
    {
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(model, validationContext, validationResults, true);

        if (validationResults.Count != 0)
        {
            var errorMessage = string.Join(" | ", validationResults.Select(x => x.ErrorMessage));
            return this.BadRequest(
                this.CreateFailureResponse(errorMessage, (int)HttpStatusCode.BadRequest)
            );
        }

        return null;
    }
}


