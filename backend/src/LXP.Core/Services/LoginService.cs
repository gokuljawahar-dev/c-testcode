namespace LXP.Core.Services;

using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _repository;
    private readonly Mapper _moviemapper;

    private readonly LXPDbContext _dbcontext;

    public LoginService(ILoginRepository repository, LXPDbContext dbcontext)
    {
        this._repository = repository;

        var _configlogin = new MapperConfiguration(cfg => cfg.CreateMap<Learner, LoginModel>());

        this._moviemapper = new Mapper(_configlogin);

        this._dbcontext = dbcontext;
    }

    public async Task<LoginRole> LoginLearner(LoginModel loginmodel)
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
            message.GetLearnerId = getlearners.LearnerId;
            if (checkpassword)
            {
                await this._repository.UpdateLearnerLastLogin(loginmodel.Email);
                loginRole = new LoginRole();

                {
                    loginRole.Email = true;

                    loginRole.Password = true;

                    loginRole.Role = message.Role;

                    loginRole.AccountStatus = message.AccountStatus;

                    loginRole.GetLearnerId = message.GetLearnerId;
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


    //    Guid learner =  _repository.FindLearnerId(emailViewModel);


    //        string password = RandomPassword.Randompasswordgenerator();
    //        string encryptPassword = Encryption.ComputePasswordToSha256Hash(password);
    //        _repository.UpdateLearnerPassword(Email, encryptPassword);
    //        EmailGenerator.Sendpassword(password, Email);


    //    else


    //        string encryptNewPassword = Encryption.ComputePasswordToSha256Hash(updatePassword.NewPassword);
    //        learner.Password = encryptNewPassword;
    //        result.success= true;

    //    else


}
