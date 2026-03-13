namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LearnerDashboardController(ILearnerDashboardService learnerDashboardService)
    : BaseController
{
    private readonly ILearnerDashboardService _learnerDashboardService = learnerDashboardService;

    [HttpGet("/lxp/learner/LearnerDashboard/{learnerId}")]
    public IActionResult GetLearnerDashboard(Guid learnerId)
    {
        var dashboard = this._learnerDashboardService.GetLearnerDashboardDetails(learnerId);

        return this.Ok(this.CreateSuccessResponse(dashboard));
    }
}
