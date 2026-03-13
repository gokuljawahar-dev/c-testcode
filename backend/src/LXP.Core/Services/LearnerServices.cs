namespace LXP.Core.Services;

using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class LearnerServices(
    ILearnerRepository courseRepository,
    IWebHostEnvironment environment,
    IHttpContextAccessor httpContextAccess
) : ILearnerServices
{
    private readonly ILearnerRepository _LearnerRepository = courseRepository;
    private readonly IWebHostEnvironment _environment = environment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccess;

    public IEnumerable<AllLearnersViewModel> GetLearners()
    {
        var result = this._LearnerRepository.GetLearners();
        return result;
    }

    public object GetAllLearnerDetailsByLearnerId(Guid learnerid) =>
        this._LearnerRepository.GetAllLearnerDetailsByLearnerId(learnerid);

    public object GetLearnerEnrolledcourseByLearnerId(Guid learnerid) =>
        this._LearnerRepository.GetLearnerEnrolledcourseByLearnerId(learnerid);
}
