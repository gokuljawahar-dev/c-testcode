namespace LXP.Core.Services;

using System.Transactions;
using LXP.Common.Constants;
using LXP.Common.Entities;
using LXP.Common.ViewModels.QuizQuestionViewModel;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

public class ExcelToJsonService(IQuizQuestionJsonRepository quizQuestionRepository)
    : IExcelToJsonService
{
    private readonly IQuizQuestionJsonRepository _quizQuestionRepository = quizQuestionRepository;

    public async Task<List<QuizQuestionJsonViewModel>> ConvertExcelToJsonAsync(IFormFile file)
    {
        var quizQuestions = new List<QuizQuestionJsonViewModel>();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using (var package = new ExcelPackage(stream))
            {
                var worksheet =
                    package.Workbook.Worksheets.FirstOrDefault()
                    ?? throw new ArgumentException("Worksheet not found.");

                for (var row = 3; row <= worksheet.Dimension.End.Row; row++)
                {
                    var questionType = worksheet
                        .Cells[row, ExcelDataExtractionColumnPositions.QuestiontypePosition]
                        .Value?.ToString();
                    var question = worksheet
                        .Cells[row, ExcelDataExtractionColumnPositions.QuestionPosition]
                        .Value?.ToString();
                    if (!string.IsNullOrEmpty(question))
                    {
                        question = question.Replace("\n", " ").Replace("\r", "");
                    }
                    if (string.IsNullOrEmpty(questionType) || string.IsNullOrEmpty(question))
                    {
                        continue;
                    }

                    var quizQuestion = new QuizQuestionJsonViewModel
                    {
                        QuestionNumber = row - 2,
                        QuestionType = questionType,
                        Question = question,
                        Options = ExtractOptions(
                            worksheet,
                            row,
                            ExcelDataExtractionColumnPositions.OptionsStartingPosition,
                            ExcelDataExtractionColumnPositions.OverallOptionsCount,
                            questionType
                        ),
                        CorrectOptions = ExtractOptions(
                            worksheet,
                            row,
                            ExcelDataExtractionColumnPositions.CorrectOptionsStartingPosition,
                            ExcelDataExtractionColumnPositions.CorrectOptionsTotalCount,
                            questionType
                        )
                    };

                    quizQuestions.Add(quizQuestion);
                }
            }
        }

        return quizQuestions;
    }


    public List<QuizQuestionJsonViewModel> ValidateQuizData(
        List<QuizQuestionJsonViewModel> quizData
    )
    {
        var validQuizData = new List<QuizQuestionJsonViewModel>();
        var invalidQuizData = new List<QuizQuestionJsonViewModel>();

        foreach (var question in quizData)
        {
            var isValid = true;

            if (question.QuestionType == QuizQuestionTypes.MultiChoiceQuestion)
            {
                if (
                    question.Options.Length
                        != ExcelDataExtractionColumnPositions.OptionsTotalCountForMCQ
                    || question.Options.Distinct().Count()
                        != ExcelDataExtractionColumnPositions.OptionsTotalCountForMCQ
                    || question.CorrectOptions.Length
                        != ExcelDataExtractionColumnPositions.CorrectOptionCountForMCQ
                    || !question.Options.Contains(question.CorrectOptions.First())
                )
                {
                    isValid = false;
                }
            }
            else if (question.QuestionType == QuizQuestionTypes.TrueFalseQuestion)
            {
                if (
                    question.Options.Length
                        != ExcelDataExtractionColumnPositions.OptionsTotalCountForTorF
                    || !question.Options.Contains("True", StringComparer.OrdinalIgnoreCase)
                    || !question.Options.Contains("False", StringComparer.OrdinalIgnoreCase)
                    || question.CorrectOptions.Length
                        != ExcelDataExtractionColumnPositions.CorrectOptionCountForTorF
                    || !question.CorrectOptions.Any(co =>
                        co.Equals("True", StringComparison.OrdinalIgnoreCase)
                        || co.Equals("False", StringComparison.OrdinalIgnoreCase)
                    )
                )
                {
                    isValid = false;
                }
            }
            else if (question.QuestionType == QuizQuestionTypes.MultiSelectQuestion)
            {
                if (
                    question.Options.Length
                        < ExcelDataExtractionColumnPositions.OptionsStartingPositionForMSQ
                    || question.Options.Length
                        > ExcelDataExtractionColumnPositions.OptionsEndingPositionForMSQ
                    || question.Options.Distinct().Count() != question.Options.Length
                    || question.CorrectOptions.Length
                        < ExcelDataExtractionColumnPositions.CorrectOptionsStartingCountForMSQ
                    || question.CorrectOptions.Length
                        > ExcelDataExtractionColumnPositions.CorrectOptionsEndingCountForMSQ
                    || !question.CorrectOptions.All(co => question.Options.Contains(co))
                )
                {
                    isValid = false;
                }
            }

            if (isValid)
            {
                validQuizData.Add(question);
            }
            else
            {
                invalidQuizData.Add(question); // Log or store invalid questions
            }
        }

        // Return or log invalid questions for further analysis if needed.
        return validQuizData;
    }

    public async Task SaveQuizDataAsync(List<QuizQuestionJsonViewModel> quizData, Guid quizId)
    {
        foreach (var quizQuestion in quizData)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var nextQuestionNo = await this._quizQuestionRepository.GetNextQuestionNoAsync(
                    quizId
                );

                var questionEntity = new QuizQuestion
                {
                    QuizId = quizId,
                    QuestionNo = nextQuestionNo,
                    QuestionType = quizQuestion.QuestionType,
                    Question = quizQuestion.Question,
                    CreatedBy = "Admin",
                    CreatedAt = DateTime.Now
                };

                await this._quizQuestionRepository.AddQuestionsAsync([questionEntity]);

                var optionEntities = quizQuestion
                    .Options.Select(
                        (option, index) =>
                            new QuestionOption
                            {
                                QuizQuestionId = questionEntity.QuizQuestionId,
                                Option = option,
                                IsCorrect = quizQuestion.CorrectOptions.Contains(option),
                                CreatedAt = DateTime.Now,
                                CreatedBy = "Admin",
                            }
                    )
                    .ToList();

                await this._quizQuestionRepository.AddOptionsAsync(
                    optionEntities,
                    questionEntity.QuizQuestionId
                );

                transaction.Complete();
            }
        }
    }


    private static string[] ExtractOptions(
        ExcelWorksheet worksheet,
        int row,
        int startColumn,
        int count,
        string questionType
    )
    {
        var options = new List<string>();
        for (var i = 0; i < count; i++)
        {
            var option = worksheet.Cells[row, startColumn + i].Value?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(option))
            {
                option = option.Replace("\n", " ").Replace("\r", " ").Trim();
                if (questionType == QuizQuestionTypes.TrueFalseQuestion)
                {
                    if (option.Equals("1", StringComparison.Ordinal))
                    {
                        option = "True";
                    }
                    else if (option.Equals("0", StringComparison.Ordinal))
                    {
                        option = "False";
                    }
                    else if (option.Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        option = "True";
                    }
                    else if (option.Equals("False", StringComparison.OrdinalIgnoreCase))
                    {
                        option = "False";
                    }
                }
                options.Add(option);
            }
        }
        return options.Where(x => !string.IsNullOrEmpty(x)).ToArray();
    }
}


//     List<QuizQuestionJsonViewModel> quizData
// )

//     foreach (var question in quizData)
//                 question.Options.Length
//                     != ExcelDataExtractionColumnPositions.OptionsTotalCountForMCQ
//                 || question.Options.Distinct().Count()
//                     != ExcelDataExtractionColumnPositions.OptionsTotalCountForMCQ
//                 || question.CorrectOptions.Length
//                     != ExcelDataExtractionColumnPositions.CorrectOptionCountForMCQ
//                 || !question.Options.Contains(question.CorrectOptions.First())
//             )
//                 continue;
//         // else if (question.QuestionType == QuizQuestionTypes.TrueFalseQuestion)
//         // {
//         //     if (
//         //         question.Options.Length
//         //             != ExcelDataExtractionColumnPositions.OptionsTotalCountForTorF
//         //         || !question.Options.Contains("True", StringComparer.OrdinalIgnoreCase)
//         //         || !question.Options.Contains("False", StringComparer.OrdinalIgnoreCase)
//         //         || question.CorrectOptions.Length
//         //             != ExcelDataExtractionColumnPositions.CorrectOptionCountForTorF
//         //         || !question.CorrectOptions.Any(co =>
//         //             co.Equals("True", StringComparison.OrdinalIgnoreCase)
//         //             || co.Equals("False", StringComparison.OrdinalIgnoreCase)
//         //         )
//         //     )
//         //     {
//         //         continue;
//         //     }
//         // }
//         else if (question.QuestionType == QuizQuestionTypes.TrueFalseQuestion)
//                 question.Options.Length
//                     != ExcelDataExtractionColumnPositions.OptionsTotalCountForTorF
//                 || !question.Options.Contains("True", StringComparer.OrdinalIgnoreCase)
//                 || !question.Options.Contains("False", StringComparer.OrdinalIgnoreCase)
//                 || question.CorrectOptions.Length
//                     != ExcelDataExtractionColumnPositions.CorrectOptionCountForTorF
//                 || !question.CorrectOptions.Any(co =>
//                     co.Equals("True", StringComparison.OrdinalIgnoreCase)
//                     || co.Equals("False", StringComparison.OrdinalIgnoreCase)
//                 )
//             )
//                 continue;
//         else if (question.QuestionType == QuizQuestionTypes.MultiSelectQuestion)
//                 question.Options.Length
//                     < ExcelDataExtractionColumnPositions.OptionsStartingPositionForMSQ
//                 || question.Options.Length
//                     > ExcelDataExtractionColumnPositions.OptionsEndingPositionForMSQ
//                 || question.Options.Distinct().Count() != question.Options.Length
//                 || question.CorrectOptions.Length
//                     < ExcelDataExtractionColumnPositions.CorrectOptionsStartingCountForMSQ
//                 || question.CorrectOptions.Length
//                     > ExcelDataExtractionColumnPositions.CorrectOptionsEndingCountForMSQ
//                 || !question.CorrectOptions.All(co => question.Options.Contains(co))
//             )
//                 continue;
//         validQuizData.Add(question);


//     ExcelWorksheet worksheet,
//     int row,
//     int startColumn,
//     int count,
//     string questionType
// )
//             option = option.Replace("\n", " ").Replace("\r", " ");
//                     option = "True";
//                 else if (option == "0")
//                     option = "False";
//             options.Add(option);
