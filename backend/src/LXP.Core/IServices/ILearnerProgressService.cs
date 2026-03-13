namespace LXP.Core.IServices;

using LXP.Common.Entities;
using LXP.Common.ViewModels;

public interface ILearnerProgressService
{
    Task<bool> Progress(LearnerProgressViewModel learnerProgress);
    Task<bool> materialCompletion(Guid learnerId, Guid materialId);
    Task<bool> UpdateProgress(Guid learnerId, Guid materialId, TimeOnly watchtime);

    //Task<double> materialCompletionPercentage(Guid learnerId, Guid materialId);

    Task<double> TopicTotalTime(Guid topicId);
    Task<double> CourseTotalTime(Guid courseId);

    // Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId);

    Task<double> TopicWatchTime(Guid topicId, Guid learnerId);
    Task<double> CourseWatchTime(Guid courseId, Guid learnerId);

    //Task CalculateAndUpdateCourseCompletionAsync(Guid learnerId);

    //  Task<decimal?> GetCourseCompletionPercentageAsync(Guid learnerId, Guid enrollmentId);

    Task<(decimal? CourseCompletionPercentage, Guid? CourseId)> GetCourseCompletionAndCourseIdAsync(
        Guid learnerId,
        Guid enrollmentId
    );
    Task<LearnerProgress> GetLearnerProgressByLearnerIdAndMaterialId(
        string LearnerId,
        string MaterialId
    );

    Task<bool> LearnerProgress(ProgressViewModel learnerProgress);

    Task<double> CalculateMaterialProgressAsync(Guid materialId, Guid learnerId);

    // Task<(decimal CombinedProgress, Guid? CourseId)> CalculateCombinedProgressAsync(Guid learnerId, Guid enrollmentId, Guid materialId);
}
//    Task Progress(LearnerProgressViewModel learnerProgress);
//    Task<double> materialCompletion(Guid learnerId, Guid materialId);
//    Task<double> materialWatchTime(Guid learnerId, Guid materialId, TimeOnly watchTime);
//    Task<double> materialCompletionPercentage(Guid learnerId, Guid materialId);

//    Task<double> TopicTotalTime(Guid topicId);
//    Task<double> CourseTotalTime(Guid courseId);
//    // Task<LearnerProgressViewModel> GetProgressById(Guid learnerProgressId);

//    Task<double> TopicWatchTime(Guid topicId, Guid learnerId);
//    Task<double> CourseWatchTime(Guid courseId, Guid learnerId);
