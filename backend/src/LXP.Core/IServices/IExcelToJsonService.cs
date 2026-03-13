namespace LXP.Core.IServices;

using LXP.Common.ViewModels.QuizQuestionViewModel;
using Microsoft.AspNetCore.Http;

public interface IExcelToJsonService
{
    Task<List<QuizQuestionJsonViewModel>> ConvertExcelToJsonAsync(IFormFile file);
    Task SaveQuizDataAsync(List<QuizQuestionJsonViewModel> quizData, Guid quizId);
    List<QuizQuestionJsonViewModel> ValidateQuizData(List<QuizQuestionJsonViewModel> quizData);
}
