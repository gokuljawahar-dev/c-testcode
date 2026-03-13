namespace LXP.Core.Services;

using LXP.Core.IServices;
using LXP.Data.IRepository;

public class LearnerAttemptServices(ILearnerAttemptRepository repository) : ILearnerAttemptServices
{
    private readonly ILearnerAttemptRepository _repository = repository;

    public object GetScoreByTopicIdAndLernerId(string LearnerId) =>
        this._repository.GetScoreByTopicIdAndLernerId(Guid.Parse(LearnerId));

    public object GetScoreByLearnerId(string LearnerId) =>
        this._repository.GetScoreByLearnerId(Guid.Parse(LearnerId));
}
