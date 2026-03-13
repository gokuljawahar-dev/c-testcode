namespace LXP.Api.Controllers;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RandomPasswordController(IService services) : ControllerBase
{
    private readonly IService _services = services;

    [HttpPost]
    public async Task<ActionResult> ForgetPassword([FromBody] RandomPasswordEmail randompassword)
    {
        var randomstore = this._services.ForgetPassword(randompassword.Email);

        return this.Ok(randomstore);
    }
}
