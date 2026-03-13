namespace LXP.Api.Controllers;

using System.Text.Json;
using LXP.Core.IServices;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExcelToJsonController(IExcelToJsonService excelToJsonService) : BaseController
{
    private readonly IExcelToJsonService _excelToJsonService = excelToJsonService;

    [HttpPost("ConvertExcelToJson")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
    [ProducesResponseType(typeof(ObjectResult), 500)]
    public async Task<IActionResult> ConvertExcelToJson(IFormFile file, Guid quizId)
    {
        if (file == null || file.Length == 0)
        {
            return this.BadRequest(
                this.CreateFailureResponse("The file is required and cannot be empty.", 400)
            );
        }

        if (quizId == Guid.Empty)
        {
            return this.BadRequest(
                this.CreateFailureResponse("The quiz ID must be a valid GUID.", 400)
            );
        }

        try
        {
            var jsonData = await this._excelToJsonService.ConvertExcelToJsonAsync(file);
            var validatedJsonData = this._excelToJsonService.ValidateQuizData(jsonData);
            await this._excelToJsonService.SaveQuizDataAsync(validatedJsonData, quizId);

            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(validatedJsonData, options);
            var byteArray = System.Text.Encoding.UTF8.GetBytes(jsonString);
            var stream = new MemoryStream(byteArray);

            return this.File(stream, "application/json", "convertedData.json");
        }
        catch (Exception ex)
        {
            return this.StatusCode(
                500,
                this.CreateFailureResponse(
                    $"An error occurred while converting Excel to JSON: {ex.Message}",
                    500
                )
            );
        }
    }
}
