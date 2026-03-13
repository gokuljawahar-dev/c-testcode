namespace LXP.Data.Repository;

using LXP.Common.Entities;
using Microsoft.EntityFrameworkCore;

public class Repository(LXPDbContext dbcontext) : IRepository.IRepository
{
    private readonly LXPDbContext _dbcontext = dbcontext;

    public async Task CheckLearner(Learner loginmodel)
    {
        var db = new LXPDbContext();

        await db.Learners.AddAsync(loginmodel);

        await db.SaveChangesAsync();
    }

    public async Task<bool> AnyUserByEmail(string loginmodel) =>
        this._dbcontext.Learners.Any(learner => learner.Email == loginmodel);

    public async Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password) =>
        await this._dbcontext.Learners.AnyAsync(learner =>
            learner.Email == Email && learner.Password == Password
        );

    public async Task<Learner> GetLearnerByEmail(string Email) =>
        await this._dbcontext.Learners.FirstOrDefaultAsync(learner => learner.Email == Email);

    public async Task UpdateLearnerPassword(string Email, string Password)
    {
        var learner = await this.GetLearnerByEmail(Email);
        learner.Password = Password;
        var passwordHistory = await this._dbcontext.PasswordHistories.FirstOrDefaultAsync(
            password => password.LearnerId == learner.LearnerId
        );
        passwordHistory.OldPassword = passwordHistory.NewPassword;
        passwordHistory.NewPassword = Password;
        this._dbcontext.PasswordHistories.Update(passwordHistory);
        this._dbcontext.Learners.Update(learner);
        await this._dbcontext.SaveChangesAsync();
    }

    public async Task UpdatePassword(Learner learner)
    {
        this._dbcontext.Learners.Update(learner);

        await this._dbcontext.SaveChangesAsync();
    }

    public async Task<Learner> LearnerByEmailAndPassword(string Email, string Password) =>
        await this._dbcontext.Learners.FirstOrDefaultAsync(learner =>
            learner.Email == Email && learner.Password == Password
        );
}
