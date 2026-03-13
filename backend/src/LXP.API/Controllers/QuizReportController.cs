namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class QuizReportController(IQuizReportServices quizReportServices) : BaseController
{
    private readonly IQuizReportServices _quizReportServices = quizReportServices;

    /// <summary>
    /// Report for Quiz
    /// </summary>
    [HttpGet("QuizReport")]
    public IActionResult GetQuizReport()
    {
        var report = this._quizReportServices.GetQuizReports();
        return this.Ok(this.CreateSuccessResponse(report));
    }

    /// <summary>
    /// GetPassdLearnersList
    /// </summary>
    [HttpGet("QuizReport/passedlearnersReport/{Quizid}!")]
    public IActionResult GetPassdLearnersList(Guid Quizid)
    {
        var PassesLearners = this._quizReportServices.GetPassdLearnersList(Quizid);
        return this.Ok(this.CreateSuccessResponse(PassesLearners));
    }

    /// <summary>
    /// GetFailedLearnersList
    /// </summary>
    [HttpGet("QuizReport/FailedlearnersReport/{Quizid}")]
    public IActionResult GetFailedLearnersList(Guid Quizid)
    {
        var PassesLearners = this._quizReportServices.GetFailedLearnersList(Quizid);
        return this.Ok(this.CreateSuccessResponse(PassesLearners));
    }
}
