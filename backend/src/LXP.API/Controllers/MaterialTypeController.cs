namespace LXP.Api.Controllers;

using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MaterialTypeController(IMaterialTypeServices materialTypeServices) : BaseController
{
    private readonly IMaterialTypeServices _materialTypeServices = materialTypeServices;

    [HttpGet("/lxp/course/materialtype")]
    public IActionResult GetAllMaterialType() =>
        this.Ok(this.CreateSuccessResponse(this._materialTypeServices.GetAllMaterialType()));
}
