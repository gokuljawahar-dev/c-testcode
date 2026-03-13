namespace LXP.Core.Services;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class QuizReportServices(IQuizReportRepository quizReportRepository) : IQuizReportServices
{
    private readonly IQuizReportRepository _quizReportRepository = quizReportRepository;

    public IEnumerable<QuizReportViewModel> GetQuizReports() =>
        this._quizReportRepository.GetQuizReports();

    public IEnumerable<QuizScorelearnerViewModel> GetPassdLearnersList(Guid Quizid) =>
        //double Passmark = _quizReportRepository.FindPassmark(Quizid);

        this._quizReportRepository.GetPassdLearnersList(Quizid);

    public IEnumerable<QuizScorelearnerViewModel> GetFailedLearnersList(Guid Quizid) =>
        this._quizReportRepository.GetFailedLearnersList(Quizid);
}
