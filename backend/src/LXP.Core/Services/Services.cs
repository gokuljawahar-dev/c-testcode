namespace LXP.Core.Services;

using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.Utils;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class Services : IService
{
    private readonly IRepository _repository;

    private readonly Mapper _moviemapper;

    public Services(IRepository repository)
    {
        this._repository = repository;

        var _configlogin = new MapperConfiguration(cfg => cfg.CreateMap<Learner, LoginModel>());

        this._moviemapper = new Mapper(_configlogin);
    }

    public async Task<LoginRole> CheckLearner(LoginModel loginmodel)
    {
        LoginRole loginRole;

        var message = new LoginRole();

        var getlearners = await this._repository.GetLearnerByEmail(loginmodel.Email);

        var user = await this._repository.AnyUserByEmail(loginmodel.Email);

        if (user)
        {
            var inputHashPassword = SHA256.HashData(Encoding.UTF8.GetBytes(loginmodel.Password));

            var stringBuilder = new StringBuilder();

            for (var i = 0; i < inputHashPassword.Length; i++)
            {
                stringBuilder.Append(inputHashPassword[i].ToString("x2"));
            }

            var inputPasswordHashed = stringBuilder.ToString();

            var checkpassword = await this._repository.AnyLearnerByEmailAndPassword(
                loginmodel.Email,
                inputPasswordHashed
            );

            message.Role = getlearners.Role;

            message.AccountStatus = getlearners.AccountStatus;

            if (checkpassword)
            {
                loginRole = new LoginRole();

                {
                    loginRole.Email = true;

                    loginRole.Password = true;

                    loginRole.Role = message.Role;

                    loginRole.AccountStatus = message.AccountStatus;
                }

                return loginRole;
            }
            else
            {
                loginRole = new LoginRole();

                {
                    loginRole.Email = true;

                    loginRole.Password = false;
                }
                return loginRole;
            }
        }
        else
        {
            loginRole = new LoginRole();

            {
                loginRole.Email = false;

                loginRole.Password = false;
            }
            return loginRole;
        }
    }

    public async Task<bool> ForgetPassword(string Email)
    {
        var getleareremail = await this._repository.AnyUserByEmail(Email);

        if (getleareremail)
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

    public async Task<bool> UpdatePassword(UpdatePassword updatePassword)
    {
        var learner = await this._repository.LearnerByEmailAndPassword(
            updatePassword.Email,
            Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword)
        );

        if (learner.Password == Encryption.ComputePasswordToSha256Hash(updatePassword.OldPassword))
        {
            var encryptNewPassword = Encryption.ComputePasswordToSha256Hash(
                updatePassword.NewPassword
            );
            learner.Password = encryptNewPassword;
            await this._repository.UpdatePassword(learner);

            return true;
        }
        else
        {
            return false;
        }
    }
}
