namespace LXP.Api.Controllers;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[Action]")]
[ApiController]
public class LoginController(ILoginService services) : ControllerBase
{
    private readonly ILoginService _services = services;

    ///<summary>
    ///Login for Leaners along with their Role (Admin and User)
    ///</summary>


    [HttpPost]
    public async Task<ActionResult> LoginLearner([FromBody] LoginModel loginmodel)
    {
        var data = await this._services.LoginLearner(loginmodel);

        return this.Ok(data);
    }

}


//    Guid Learnerid = await _services.GetLearnerId(emailViewModel);

