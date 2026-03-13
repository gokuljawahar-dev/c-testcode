namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IForgetRepository
{

    public bool AnyUserByEmail(string loginmodel);


    public Learner GetLearnerByEmail(string Email);

    public void UpdateLearnerPassword(string Email, string Password);


}
