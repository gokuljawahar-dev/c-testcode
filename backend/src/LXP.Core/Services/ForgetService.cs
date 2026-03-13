namespace LXP.Core.Services;

using LXP.Common.Utils;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class ForgetService(IForgetRepository repository) : IForgetService
{
    private readonly IForgetRepository _repository = repository;

    public bool ForgetPassword(string Email)
    {
        var getleareremail = this._repository.AnyUserByEmail(Email);

        if (getleareremail != null)
        {
            var password = RandomPassword.Randompasswordgenerator();
            var encryptPassword = Encryption.ComputePasswordToSha256Hash(password);
            this._repository.UpdateLearnerPassword(Email, encryptPassword);
            EmailGenerator.Sendpassword(password, Email);
            return true;
        }
        else
        {
            return false;
        }
    }
}
