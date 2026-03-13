namespace LXP.Data.IRepository;

using LXP.Common.ViewModels;

public interface IUserReportRepository
{
    IEnumerable<UserReportViewModel> GetUserReport();
}
