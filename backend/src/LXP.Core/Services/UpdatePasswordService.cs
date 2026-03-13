namespace LXP.Core.Services;

using LXP.Common.Utils;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class UpdatePasswordService(IUpdatePasswordRepository repository) : IUpdatePasswordService
{
    private readonly IUpdatePasswordRepository _repository = repository;

    public async Task<bool> UpdatePassword(UpdatePassword updatePassword)
    {
        var learner = await this._repository.LearnerByEmailAndPasswordAsync(
            updatePassword.Email,
            Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword)
        );

        if (learner == null)
        {
            return false;
        }

        var encryptNewPassword = Encryption.ComputePasswordToSha256Hash(updatePassword.NewPassword);
        learner.Password = encryptNewPassword;

        await this._repository.UpdatePasswordAsync(learner);
        return true;
    }
}
