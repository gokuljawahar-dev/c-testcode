namespace LXP.Core.IServices;

using LXP.Common.ViewModels.QuizQuestionViewModel;

public interface IQuizQuestionService
{
    Task<Guid> AddQuestionAsync(
        QuizQuestionViewModel quizQuestion,
        List<QuestionOptionViewModel> options
    );
    Task<bool> UpdateQuestionAsync(
        Guid quizQuestionId,
        QuizQuestionViewModel quizQuestion,
        List<QuestionOptionViewModel> options
    );
    Task<bool> DeleteQuestionAsync(Guid quizQuestionId);
    Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId);
    Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync();
    Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId);
}
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using LXP.Common.ViewModels.QuizQuestionViewModel;

//namespace LXP.Core.IServices
//        Task<Guid> AddQuestionAsync(
//            QuizQuestionViewModel quizQuestion,
//            List<QuestionOptionViewModel> options
//        );
//        Task<bool> UpdateQuestionAsync(
//            Guid quizQuestionId,
//            QuizQuestionViewModel quizQuestion,
//            List<QuestionOptionViewModel> options
//        );
//        Task<bool> DeleteQuestionAsync(Guid quizQuestionId);
//        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsByQuizIdAsync(Guid quizId);
//        Task<List<QuizQuestionNoViewModel>> GetAllQuestionsAsync();
//        Task<QuizQuestionNoViewModel> GetQuestionByIdAsync(Guid quizQuestionId);
