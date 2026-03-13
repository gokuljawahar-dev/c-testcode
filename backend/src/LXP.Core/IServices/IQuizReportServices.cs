namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface IQuizReportServices
{
    IEnumerable<QuizReportViewModel> GetQuizReports();
    IEnumerable<QuizScorelearnerViewModel> GetPassdLearnersList(Guid Quizid);
    IEnumerable<QuizScorelearnerViewModel> GetFailedLearnersList(Guid Quizid);
}
