namespace LXP.Core.IServices;

using LXP.Common.Entities;
using LXP.Common.ViewModels;

public interface ILearnerService
{
    Task<bool> LearnerRegistration(RegisterUserViewModel registerUserViewModel);

    Task<List<GetLearnerViewModel>> GetAllLearner();

    //Task<List<Learner>>Updateall

    Learner GetLearnerById(string id);
    Task<LearnerAndProfileViewModel> LearnerGetLearnerById(string id);
}
