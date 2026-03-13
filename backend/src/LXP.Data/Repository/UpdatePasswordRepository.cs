namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class UpdatePasswordRepository(LXPDbContext dbcontext) : IUpdatePasswordRepository
{
    private readonly LXPDbContext _dbcontext = dbcontext;

    public async Task<Learner> LearnerByEmailAndPasswordAsync(string email, string password) =>
        await this._dbcontext.Learners.FirstOrDefaultAsync(learner =>
            learner.Email == email && learner.Password == password
        );

    public async Task UpdatePasswordAsync(Learner learner)
    {
        var passwordHistory = await this._dbcontext.PasswordHistories.FirstOrDefaultAsync(
            password => password.LearnerId == learner.LearnerId
        );
        passwordHistory.OldPassword = passwordHistory.NewPassword;
        passwordHistory.NewPassword = learner.Password;
        this._dbcontext.PasswordHistories.Update(passwordHistory);
        this._dbcontext.Learners.Update(learner);
        await this._dbcontext.SaveChangesAsync();
    }
}
