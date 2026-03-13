namespace LXP.Core.IServices;

public interface ILearnerAttemptServices
{
    object GetScoreByTopicIdAndLernerId(string LearnerId);

    object GetScoreByLearnerId(string LearnerId);
}
