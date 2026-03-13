namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface IUserReportServices
{
    IEnumerable<UserReportViewModel> GetUserReport();
}
