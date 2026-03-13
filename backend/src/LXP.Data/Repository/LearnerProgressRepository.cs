namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class LearnerProgressRepository(LXPDbContext context) : ILearnerProgressRepository
{
    private readonly LXPDbContext _context = context;

    public async Task LearnerProgress(LearnerProgress learnerProgress)
    {
        await this._context.LearnerProgresses.AddAsync(learnerProgress);
        await this._context.SaveChangesAsync();
    }

    public async Task<bool> AnyLearnerProgressByLearnerIdAndMaterialId(
        Guid LearnerId,
        Guid MaterialId
    ) =>
        await this._context.LearnerProgresses.AnyAsync(learnerProgress =>
            learnerProgress.MaterialId == MaterialId && learnerProgress.LearnerId == LearnerId
        );

    public async Task<LearnerProgress> GetLearnerProgressByLearnerIdAndMaterialId(
        Guid LearnerId,
        Guid MaterialId
    ) =>
        await this._context.LearnerProgresses.FirstAsync(learnerProgress =>
            learnerProgress.MaterialId == MaterialId && learnerProgress.LearnerId == LearnerId
        );

    public async Task<LearnerProgress> GetLearnerProgressById(Guid learnerId, Guid courseId) =>
        await this._context.LearnerProgresses.FirstOrDefaultAsync(progress =>
            progress.LearnerId == learnerId && progress.CourseId == courseId
        );

    public async Task<LearnerProgress> GetLearnerProgressByMaterialId(
        Guid learnerId,
        Guid materialId
    ) =>
        await this._context.LearnerProgresses.FirstOrDefaultAsync(progress =>
            progress.LearnerId == learnerId && progress.MaterialId == materialId
        );

    public void UpdateLearnerProgress(LearnerProgress progress)
    {
        this._context.LearnerProgresses.Update(progress);
        this._context.SaveChanges();
    }

    public async Task<List<LearnerProgress>> GetMaterialByTopic(Guid topicId, Guid learnerId) =>
        await this
            ._context.LearnerProgresses.Where(material =>
                material.TopicId == topicId && material.LearnerId == learnerId
            )
            .ToListAsync();

    public async Task CalculateAndUpdateCourseCompletionAsync(Guid learnerId)
    {
        var enrollments = await this
            ._context.Enrollments.Where(e => e.LearnerId == learnerId)
            .Select(e => new LearnerProgressEnrollmentViewModel
            {
                EnrollmentId = e.EnrollmentId,
                LearnerId = e.LearnerId,
                CourseId = e.CourseId,
                EnrollmentDate = e.EnrollmentDate,
                EnrollStatus = e.EnrollStatus,
                Course = new LearnerProgressCourseViewModel
                {
                    CourseId = e.Course.CourseId,
                    LevelId = e.Course.LevelId,
                    CategoryId = e.Course.CategoryId,
                    Title = e.Course.Title,
                    Description = e.Course.Description,
                    Duration = e.Course.Duration,
                    Thumbnail = e.Course.Thumbnail,
                    IsActive = e.Course.IsActive,
                    IsAvailable = e.Course.IsAvailable,
                    CreatedBy = e.Course.CreatedBy,
                    CreatedAt = e.Course.CreatedAt,
                    ModifiedBy = e.Course.ModifiedBy,
                    ModifiedAt = e.Course.ModifiedAt,
                    Topics = e
                        .Course.Topics.Select(t => new LearnerProgressTopicViewModel
                        {
                            TopicId = t.TopicId,
                            Name = t.Name,
                            Materials = t
                                .Materials.Select(m => new LearnerProgressMaterialViewModel
                                {
                                    MaterialId = m.MaterialId,
                                    Name = m.Name,
                                    FilePath = m.FilePath,
                                    Duration = m.Duration,
                                    IsActive = m.IsActive,
                                    IsAvailable = m.IsAvailable
                                })
                                .ToList()
                        })
                        .ToList()
                }
            })
            .ToListAsync();

        foreach (var enrollment in enrollments)
        {
            var course = enrollment.Course;

            // Total course duration in hours
            var totalCourseDuration = course
                .Topics.SelectMany(t => t.Materials)
                .Sum(m => m.Duration.ToTimeSpan().TotalHours);

            // Fetch learner progress records
            var learnerProgresses = await this
                ._context.LearnerProgresses.Where(lp =>
                    lp.LearnerId == learnerId && lp.CourseId == course.CourseId
                )
                .ToListAsync();

            // Calculate watched duration in hours
            var watchedDuration = learnerProgresses
                .Where(lp => lp.IsWatched == 1)
                .Sum(lp => lp.CourseWatchtime.ToTimeSpan().TotalHours);

            // Ensure totalCourseDuration is not zero to avoid division by zero
            if (totalCourseDuration == 0)
            {
                totalCourseDuration = 1; // or handle the case appropriately
            }

            // Get the learner's last attempt score from the learner attempt table
            var lastAttempt = await this
                ._context.LearnerAttempts.Where(la =>
                    la.LearnerId == learnerId && la.Quiz.CourseId == course.CourseId
                )
                .OrderByDescending(la => la.EndTime)
                .FirstOrDefaultAsync();

            var quizScore = lastAttempt != null ? lastAttempt.Score : 0;

            // Maximum quiz score (assuming max score is 100)

            var Findpassmark = this._context.Quizzes.Find(lastAttempt!.QuizId);

            var maxQuizScore = Findpassmark!.PassMark;

            // Ensure maxQuizScore is not zero to avoid division by zero
            if (quizScore > maxQuizScore)
            {
                var materialCompletionPercentage = watchedDuration / totalCourseDuration * 70;
                var quizCompletionPercentage = 30;
                var courseCompletionPercentage =
                    materialCompletionPercentage + quizCompletionPercentage;

                // Cap the course completion percentage at 100%
                courseCompletionPercentage = Math.Min(courseCompletionPercentage, 100);

                // Update the enrollment table with the calculated percentage
                var enrollmentToUpdate = await this._context.Enrollments.FindAsync(
                    enrollment.EnrollmentId
                );
                if (enrollmentToUpdate != null)
                {
                    enrollmentToUpdate.CourseCompletionPercentage =
                        (decimal)courseCompletionPercentage;
                    if (enrollmentToUpdate.CourseCompletionPercentage == 100)
                    {
                        enrollmentToUpdate.CompletedStatus = 1;
                    }
                    this._context.Enrollments.Update(enrollmentToUpdate);
                }
            }

            // Calculate the course completion percentage
        }

        await this._context.SaveChangesAsync();
    }

    //    // Fetch the learner's enrolled courses and map to ViewModel
    //        .Where(e => e.LearnerId == learnerId)
    //        .Select(e => new LearnerProgressEnrollmentViewModel
    //            EnrollmentId = e.EnrollmentId,
    //            LearnerId = e.LearnerId,
    //            CourseId = e.CourseId,
    //            EnrollmentDate = e.EnrollmentDate,
    //            EnrollStatus = e.EnrollStatus,

    //            Course = new LearnerProgressCourseViewModel
    //                CourseId = e.Course.CourseId,
    //                LevelId = e.Course.LevelId,
    //                CategoryId = e.Course.CategoryId,
    //                Title = e.Course.Title,
    //                Description = e.Course.Description,
    //                Duration = e.Course.Duration,
    //                Thumbnail = e.Course.Thumbnail,
    //                IsActive = e.Course.IsActive,
    //                IsAvailable = e.Course.IsAvailable,
    //                CreatedBy = e.Course.CreatedBy,
    //                CreatedAt = e.Course.CreatedAt,
    //                ModifiedBy = e.Course.ModifiedBy,
    //                ModifiedAt = e.Course.ModifiedAt,
    //                Topics = e.Course.Topics.Select(t => new LearnerProgressTopicViewModel
    //                    TopicId = t.TopicId,
    //                    Name = t.Name,
    //                    Materials = t.Materials.Select(m => new LearnerProgressMaterialViewModel
    //                        MaterialId = m.MaterialId,
    //                        Name = m.Name,
    //                        FilePath = m.FilePath,
    //                        Duration = m.Duration,
    //                        IsActive = m.IsActive,
    //                        IsAvailable = m.IsAvailable

    //    foreach (var enrollment in enrollments)

    //        // Get the total number of materials for the course

    //        // Check material completion status from the learner progress table
    //            .Where(lp => lp.LearnerId == learnerId && lp.CourseId == course.CourseId && lp.IsWatched)
    //            .CountAsync();

    //        // Get the learner's last attempt score from the learner attempt table
    //            .Where(la => la.LearnerId == learnerId && la.Quiz.CourseId == course.CourseId)
    //            .OrderByDescending(la => la.EndTime)
    //            .FirstOrDefaultAsync();


    //        // Calculate the course completion percentage
    //        //var materialCompletionPercentage = (double)completedMaterials / totalMaterials * 70;
    //        //var quizCompletionPercentage = quizScore * 0.3;
    //        //var courseCompletionPercentage = materialCompletionPercentage + quizCompletionPercentage;

    //        // Update the enrollment table with the calculated percentage
    //            enrollmentToUpdate.CourseCompletionPercentage = (decimal)courseCompletionPercentage;
    //            _context.Enrollments.Update(enrollmentToUpdate);


    public async Task<Enrollment> GetEnrollmentByIdAsync(Guid learnerId, Guid enrollmentId) =>
        await this._context.Enrollments.FirstOrDefaultAsync(e =>
            e.LearnerId == learnerId && e.EnrollmentId == enrollmentId
        );

    public async Task<LearnerProgress> GetLearnerMaterialProgressAsync(
        Guid materialId,
        Guid learnerId
    ) =>
        await this._context.LearnerProgresses.FirstOrDefaultAsync(lp =>
            lp.MaterialId == materialId && lp.LearnerId == learnerId
        );

    public async Task Changewatchtime(LearnerProgress learnerprogress)
    {
        this._context.LearnerProgresses.Update(learnerprogress);
        await this._context.SaveChangesAsync();
    }
}
