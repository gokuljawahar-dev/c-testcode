namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CourseLevelController(ICourseLevelServices courseLevelServices) : BaseController
{
    private readonly ICourseLevelServices _courseLevelServices = courseLevelServices;

    [HttpGet("/lxp/course/courselevel/{id}")]
    public async Task<IActionResult> GetAllCourseLevel(string id) =>
        this.Ok(this.CreateSuccessResponse(await this._courseLevelServices.GetAllCourseLevel(id)));
}
