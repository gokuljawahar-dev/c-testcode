namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[Action]")]
[ApiController]
public class LearnerAttemptController(ILearnerAttemptServices services) : BaseController
{
    private readonly ILearnerAttemptServices _services = services;

    /// <summary>
    ///  Getting score by Topic Id and Learner ID  ---------------Ruban code
    /// </summary>

    [HttpGet]
    public IActionResult GetScoreByTopicIdAndLearnerId(string LearnerId) =>
        this.Ok(this.CreateSuccessResponse(this._services.GetScoreByTopicIdAndLernerId(LearnerId)));

    [HttpGet]
    public IActionResult GetScoreByLearnerId(string LearnerId) =>
        this.Ok(this.CreateSuccessResponse(this._services.GetScoreByLearnerId(LearnerId)));
}
