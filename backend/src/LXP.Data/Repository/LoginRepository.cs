namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class LoginRepository(LXPDbContext dbcontext) : ILoginRepository
{
    private readonly LXPDbContext _dbcontext = dbcontext;

    public async Task LoginLearner(Learner loginmodel)
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
        this._dbcontext.Learners.Update(learner);
        await this._dbcontext.SaveChangesAsync();
    }

    public async Task UpdateLearnerLastLogin(string Email)
    {
        var learners = await this._dbcontext.Learners.FirstOrDefaultAsync(learners =>
            learners.Email == Email
        );

        if (learners != null)
        {
            learners.UserLastLogin = DateTime.Now;
            this._dbcontext.Learners.Update(learners);
            await this._dbcontext.SaveChangesAsync();
        }
    }

    //    _dbcontext.Learners.Update(learner);


}
