namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface ILearnerProgressRepository
{
    Task LearnerProgress(LearnerProgress learnerProgress);

    Task<LearnerProgress> GetLearnerProgressById(Guid learnerId, Guid courseId);
    Task<LearnerProgress> GetLearnerProgressByMaterialId(Guid learnerId, Guid materialId);

    void UpdateLearnerProgress(LearnerProgress progress);
    Task<LearnerProgress> GetLearnerProgressByLearnerIdAndMaterialId(
        Guid LearnerId,
        Guid MaterialId
    );

    Task<List<LearnerProgress>> GetMaterialByTopic(Guid topicId, Guid learnerId);
    Task CalculateAndUpdateCourseCompletionAsync(Guid learnerId);

    Task<Enrollment> GetEnrollmentByIdAsync(Guid learnerId, Guid enrollmentId);

    Task<bool> AnyLearnerProgressByLearnerIdAndMaterialId(Guid LearnerId, Guid MaterialId);

    Task<LearnerProgress> GetLearnerMaterialProgressAsync(Guid materialId, Guid learnerId);

    Task Changewatchtime(LearnerProgress learnerprogress);
}
