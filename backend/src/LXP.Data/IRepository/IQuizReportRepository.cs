namespace LXP.Data.IRepository;

using LXP.Common.ViewModels;

public interface IQuizReportRepository
{
    IEnumerable<QuizReportViewModel> GetQuizReports();
    IEnumerable<QuizScorelearnerViewModel> GetPassdLearnersList(Guid Quizid);
    IEnumerable<QuizScorelearnerViewModel> GetFailedLearnersList(Guid Quizid);
}
