namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface IService
{
    public Task<LoginRole> CheckLearner(LoginModel loginmodel);

    Task<bool> ForgetPassword(string Email);

    Task<bool> UpdatePassword(UpdatePassword updatePassword);
}
