namespace LXP.Api.Controllers;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UpdatePasswordController(IUpdatePasswordService services) : BaseController
{
    private readonly IUpdatePasswordService _services = services;

    ///<summary>
    ///Update Password once user use the Forgot Password operation
    ///</summary>
    [HttpPut]
    public async Task<IActionResult> LeanerUpdatePassword([FromBody] UpdatePassword updatepassword)
    {
        var result = await this._services.UpdatePassword(updatepassword);
        if (result)
        {
            return this.Ok("Password Updated Successfully");
        }
        else
        {
            return this.Ok("Incorrect Received Password");
        }
    }
}
