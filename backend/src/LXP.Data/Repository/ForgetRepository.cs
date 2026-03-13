namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class ForgetRepository(LXPDbContext dbcontext) : IForgetRepository
{
    private readonly LXPDbContext _dbcontext = dbcontext;

    public bool AnyUserByEmail(string loginmodel) =>
        this._dbcontext.Learners.Any(learner => learner.Email == loginmodel);

    public Learner GetLearnerByEmail(string Email) =>
        this._dbcontext.Learners.FirstOrDefault(learner => learner.Email == Email);

    public void UpdateLearnerPassword(string Email, string Password)
    {
        var learner = this.GetLearnerByEmail(Email);
        learner.Password = Password;
        this._dbcontext.Learners.Update(learner);
        this._dbcontext.SaveChangesAsync();
    }

    //    _dbcontext.Learners.Update(learner);


}
