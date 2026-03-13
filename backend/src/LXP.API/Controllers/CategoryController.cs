namespace LXP.Api.Controllers;

using System.Net;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

///<summary>
///Category of the course
///</summary>
[Route("api/[controller]")]
[ApiController]
public class CategoryController(ICategoryServices categoryServices) : BaseController
{
    private readonly ICategoryServices _categoryServices = categoryServices;

    ///<summary>
    ///getting all category name and its id
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="500">Internal server Error</response>
    [HttpGet("/lxp/course/category")]
    public async Task<IActionResult> GetAllCategory()
    {
        var categories = await this._categoryServices.GetAllCategory();
        return this.Ok(this.CreateSuccessResponse(categories));
    }

    ///<summary>
    ///adding new category
    ///</summary>
    ///<response code="200">Success</response>
    ///<response code="422">Sourse is already exists</response>
    ///<response code="500">Internal server Error</response>
    [HttpPost("/lxp/course/category")]
    public async Task<IActionResult> PostCategory(CourseCategoryViewModel category)
    {
        var categoryExists = await this._categoryServices.AddCategory(category);
        if (categoryExists)
        {
            var categories = await this._categoryServices.GetAllCategory();
            return this.Ok(this.CreateSuccessResponse(categories));
        }
        else
        {
            return this.Ok(
                this.CreateFailureResponse(
                    MessageConstants.MsgAlreadyExists,
                    (int)HttpStatusCode.PreconditionFailed
                )
            );
        }
    }
}
