namespace LXP.Data.IRepository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;

public interface ILearnerRepository
{
    void AddLearner(Learner learner);
    Task<bool> AnyLearnerByEmail(string email);
    Learner GetLearnerByLearnerEmail(string email);

    Task<List<Learner>> GetAllLearner();

    //Task UpdateAllLearner(Learner learner);

    Learner GetLearnerDetailsByLearnerId(Guid LearnerId);

    Task UpdateLearner(Learner learner);

    public IEnumerable<AllLearnersViewModel> GetLearners();

    object GetAllLearnerDetailsByLearnerId(Guid learnerId);

    object GetLearnerEnrolledcourseByLearnerId(Guid learnerId);
    IDbContextTransaction BeginTransaction();
}
