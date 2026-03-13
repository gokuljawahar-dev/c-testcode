namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IUpdatePasswordRepository
{


    //  Task<bool> Changecoursestatus(Coursestatus status);

    public Task UpdatePasswordAsync(Learner learner);
    Task<Learner> LearnerByEmailAndPasswordAsync(string email, string password);
}


// Learner UpdatePasswordAsync(string Email, string Password);
