namespace LXP.Api.Controllers;

using System.Net;
using LXP.Common.Constants;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MaterialController(IMaterialServices materialService) : BaseController
{
    private readonly IMaterialServices _materialService = materialService;

    [HttpPost("/lxp/course/material")]
    public async Task<IActionResult> AddMaterial([FromForm] MaterialViewModel material)
    {
        var createdMaterial = await this._materialService.AddMaterial(material);
        if (createdMaterial != null)
        {
            return this.Ok(this.CreateInsertResponse(createdMaterial));
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

    [HttpGet("/lxp/course/topic/{topicId}/materialtype/{materialTypeId}/")]
    public async Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndMaterialType(
        string topicId,
        string materialTypeId
    ) => await this._materialService.GetAllMaterialDetailsByTopicAndType(topicId, materialTypeId);

    ///<summary>
    /// updating the material
    ///</summary>
    ///  ///<response code="200">Success</response>
    ///<response code="422">Sourse is already exists</response>
    ///<response code="500">Internal server Error</response>

    [HttpPut("/lxp/course/material")]
    public async Task<IActionResult> UpdateMaterial([FromForm] MaterialUpdateViewModel material)
    {
        var isMaterialUpdated = await this._materialService.UpdateMaterial(material);

        if (isMaterialUpdated)
        {
            return this.Ok(this.CreateSuccessResponse());
        }

        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgAlreadyExists,
                (int)HttpStatusCode.PreconditionFailed
            )
        );
    }

    ///<summary>
    /// deleting the material using materialId
    ///</summary>
    ///        /// ///<response code="200">Success</response>
    ///<response code="500">Internal server Error</response>
    [HttpDelete("/lxp/course/material/{materialId}")]
    public async Task<IActionResult> DeleteCourseMaterial(string materialId)
    {
        var deletedStatus = await this._materialService.SoftDeleteMaterial(materialId);
        if (deletedStatus)
        {
            return this.Ok(this.CreateSuccessResponse());
        }
        return this.Ok(
            this.CreateFailureResponse(
                MessageConstants.MsgNotDeleted,
                (int)HttpStatusCode.MethodNotAllowed
            )
        );
    }

    ///<summary>
    /// getting the particular material using materialId
    ///</summary>
    /// ///<response code="200">Success</response>
    ///<response code="500">Internal server Error</response>


    [HttpGet("/lxp/course/material/{materialId}")]
    public async Task<IActionResult> GetMaterialByMaterialId(string materialId)
    {
        var material = await this._materialService.GetMaterialDetailsByMaterialId(materialId);
        return this.Ok(this.CreateSuccessResponse(material));
    }

    [HttpGet("/lxp/course/material/withoutpdfconversion/{materialId}")]
    public async Task<IActionResult> GetMaterialByMaterialIdWithoutPDFConversionForUpdate(
        string materialId
    )
    {
        var material =
            await this._materialService.GetMaterialDetailsByMaterialIdWithoutPDFConversionForUpdate(
                materialId
            );
        return this.Ok(this.CreateSuccessResponse(material));
    }
}
