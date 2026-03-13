namespace LXP.Api.Controllers;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ForgetPasswordController(IForgetService services) : ControllerBase
{
    private readonly IForgetService _services = services;

    ///<summary>
    ///Forget Password with Random passwordgenerator that sends to user Email
    ///</summary>


    [HttpPost]
    public async Task<ActionResult> ForgetPassword([FromBody] RandomPasswordEmail randompassword)
    {
        var randomstore = this._services.ForgetPassword(randompassword.Email);

        return this.Ok(randomstore);
    }
}
