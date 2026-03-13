namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface ILearnerServices
{
    public IEnumerable<AllLearnersViewModel> GetLearners();
    object GetAllLearnerDetailsByLearnerId(Guid learnerid);

    object GetLearnerEnrolledcourseByLearnerId(Guid learnerid);
}
