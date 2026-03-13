namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface ILoginService
{
    public Task<LoginRole> LoginLearner(LoginModel loginmodel);

    //Task<bool> ForgetPassword(string Email);


    //Task<ResultUpdatePassword> UpdatePassword(UpdatePassword updatePassword);
}
