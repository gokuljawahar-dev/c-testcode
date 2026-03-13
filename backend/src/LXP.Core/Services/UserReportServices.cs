namespace LXP.Core.Services;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class UserReportServices(IUserReportRepository userReportRepository) : IUserReportServices
{
    private readonly IUserReportRepository _userReportRepository = userReportRepository;

    public IEnumerable<UserReportViewModel> GetUserReport() =>
        this._userReportRepository.GetUserReport();
}
