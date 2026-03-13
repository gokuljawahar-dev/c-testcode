namespace LXP.Data.IRepository;

public interface ILearnerAttemptRepository
{
    object GetScoreByTopicIdAndLernerId(Guid LearnerId);

    object GetScoreByLearnerId(Guid LearnerId);
}
