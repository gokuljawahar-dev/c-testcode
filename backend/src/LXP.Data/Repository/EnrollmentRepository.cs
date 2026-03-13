namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class EnrollmentRepository(
    LXPDbContext lXPDbContext,
    IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor
) : IEnrollmentRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;
    private readonly IWebHostEnvironment _environment = webHostEnvironment;
    private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

    public async Task Addenroll(Enrollment enrollment)
    {
        await this._lXPDbContext.Enrollments.AddAsync(enrollment);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public bool AnyEnrollmentByLearnerAndCourse(Guid learnerId, Guid courseId) =>
        this._lXPDbContext.Enrollments.Any(enrollment =>
            enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
        );

    public object GetCourseandTopicsByLearnerId(Guid learnerId)
    {
        var result =
            from enrollment in this._lXPDbContext.Enrollments
            where enrollment.LearnerId == learnerId
            select new
            {
                enrollmentid = enrollment.EnrollmentId,
                enrolledCourseId = enrollment.CourseId,
                enrolledCoursename = enrollment.Course.Title,
                completedStatus = enrollment.CompletedStatus,
                courseStarted = enrollment.CourseStarted,
                enrolledcoursedescription = enrollment.Course.Description,
                enrolledcoursecategory = enrollment.Course.Category.Category,
                enrolledcourselevels = enrollment.Course.Level.Level,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    enrollment.Course.Thumbnail
                ),

                Topics = (
                    from topic in this._lXPDbContext.Topics
                    where topic.CourseId == enrollment.CourseId && topic.IsActive
                    select new
                    {
                        TopicName = topic.Name,
                        TopicDescription = topic.Description,
                        topic.TopicId,
                        TopicIsActive = topic.IsActive,
                        Materials = (
                            from material in this._lXPDbContext.Materials
                            join materialType in this._lXPDbContext.MaterialTypes
                                on material.MaterialTypeId equals materialType.MaterialTypeId

                            where material.TopicId == topic.TopicId
                            select new
                            {
                                material.MaterialId,
                                MaterialName = material.Name,
                                MaterialType = materialType.Type,
                                Material = string.Format(
                                    "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                                    this._contextAccessor.HttpContext.Request.Scheme,
                                    this._contextAccessor.HttpContext.Request.Host,
                                    this._contextAccessor.HttpContext.Request.PathBase,
                                    material.FilePath
                                ),
                                MaterialDuration = material.Duration
                            }
                        ).ToList(),
                        //MaterialType =(from materialType in _lXPDbContext.MaterialTypes select new
                        //    MaterialType=materialType.Type,
                        //    MaterialTypeId=materialType.MaterialTypeId,

                    }
                ).ToList()
            };
        return result;
    }

    public IEnumerable<EnrollmentReportViewModel> GetEnrollmentReport()
    {
        var course = this
            ._lXPDbContext.Enrollments.GroupBy(x => x.CourseId)
            .Select(x => new EnrollmentReportViewModel
            {
                CourseId = x.First().CourseId,
                CourseName = x.First().Course.Title,
                EnrolledUsers = x.GroupBy(x => x.LearnerId).Count(),
                InprogressUsers = x.Count(x => x.CompletedStatus == 0),
                CompletedUsers = x.Count(x => x.CompletedStatus == 1),
            })
            .ToList();
        return course;
    }

    public IEnumerable<EnrolledUserViewModel> GetEnrolledUser(Guid courseId)
    {
        var users = this
            ._lXPDbContext.Enrollments.Where(x => x.CourseId == courseId)
            .Select(x => new EnrolledUserViewModel
            {
                LearnerId = x.LearnerId,
                Name =
                    x.Learner.LearnerProfiles.First().FirstName
                    + " "
                    + x.Learner.LearnerProfiles.First().LastName,
                ProfilePhoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    x.Learner.LearnerProfiles.First().ProfilePhoto
                ),
                Status = x.CompletedStatus,
                EmailId = x.Learner.Email,
            });
        return users;
    }

    public IEnumerable<EnrollmentReportViewModel> GetEnrolledCompletedLearnerbyCourseId(
        Guid courseId
    )
    {
        var CompletedLearner = this
            ._lXPDbContext.Enrollments.Where(e => e.CourseId == courseId && e.CompletedStatus == 1)
            .GroupBy(e => e.LearnerId)
            .Select(e => new EnrollmentReportViewModel
            {
                CourseId = e.First().CourseId,
                LearnerId = e.Key,
                LearnerName = e.First().Learner.LearnerProfiles.First().FirstName,
                ProfilePhoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext!.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    e.First().Learner.LearnerProfiles.First().ProfilePhoto
                ),
                EmailId = e.First().Learner.Email,
                CourseCompletionPercentage = e.First().CourseCompletionPercentage,
            })
            .ToList();
        return CompletedLearner;
    }

    public IEnumerable<EnrollmentReportViewModel> GetEnrolledInprogressLearnerbyCourseId(
        Guid courseId
    )
    {
        var InprogressLearner = this
            ._lXPDbContext.Enrollments.Where(e => e.CourseId == courseId && e.CompletedStatus != 1)
            .GroupBy(e => e.LearnerId)
            .Select(e => new EnrollmentReportViewModel
            {
                CourseId = e.First().CourseId,
                LearnerId = e.Key,
                LearnerName = e.First().Learner.LearnerProfiles.First().FirstName,
                ProfilePhoto = string.Format(
                    "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                    this._contextAccessor.HttpContext!.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    e.First().Learner.LearnerProfiles.First().ProfilePhoto
                ),
                EmailId = e.First().Learner.Email,
                CourseCompletionPercentage = e.First().CourseCompletionPercentage,
            })
            .ToList();
        return InprogressLearner;
    }

    public Enrollment FindEnrollmentId(Guid enrollmentId) =>
        this._lXPDbContext.Enrollments.Find(enrollmentId);

    public async Task DeleteEnrollment(Enrollment enrollment)
    {
        this._lXPDbContext.Enrollments.Remove(enrollment);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task UpdateCourseStarted(Enrollment enrollment)
    {
        this._lXPDbContext.Enrollments.Update(enrollment);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public object GetCourseandTopicsByCourseIdAndLearnerId(Guid courseId, Guid learnerId)
    {
        var result =
            from enrollment in this._lXPDbContext.Enrollments
            where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
            select new
            {
                enrollmentid = enrollment.EnrollmentId,
                enrolledCourseId = enrollment.CourseId,
                enrolledCoursename = enrollment.Course.Title,
                enrolledcoursedescription = enrollment.Course.Description,
                enrolledcoursecategory = enrollment.Course.Category.Category,
                enrolledcourselevels = enrollment.Course.Level.Level,
                Thumbnailimage = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    enrollment.Course.Thumbnail
                ),

                Topics = (
                    from topic in this._lXPDbContext.Topics
                    where topic.CourseId == enrollment.CourseId && topic.IsActive
                    select new
                    {
                        TopicName = topic.Name,
                        TopicDescription = topic.Description,
                        topic.TopicId,
                        TopicIsActive = topic.IsActive,
                        IsQuiz = this._lXPDbContext.Quizzes.Any(quizzes =>
                            quizzes.TopicId == topic.TopicId
                        )
                            && (
                                from q in this._lXPDbContext.Quizzes
                                join la in this._lXPDbContext.LearnerAttempts
                                    on q.QuizId equals la.QuizId
                                where la.LearnerId == learnerId && q.TopicId == topic.TopicId
                                group la by new { la.QuizId, q.PassMark } into g
                                where g.Max(x => x.Score) >= g.Key.PassMark
                                select g.Key.PassMark
                            ).Count() == 0,
                        IsFeedBack = this._lXPDbContext.TopicFeedbackQuestions.Any(
                            topicfeedbackquesion => topicfeedbackquesion.TopicId == topic.TopicId
                        )
                            && (
                                from tfq in this._lXPDbContext.TopicFeedbackQuestions
                                join fr in this._lXPDbContext.FeedbackResponses
                                    on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
                                where fr.LearnerId == learnerId && tfq.TopicId == topic.TopicId
                                select tfq
                            ).Count() == 0,
                        isAttemptOver = (
                            from quiz in this._lXPDbContext.Quizzes
                            join attempt in this._lXPDbContext.LearnerAttempts
                                on quiz.QuizId equals attempt.QuizId
                            where attempt.LearnerId == learnerId && quiz.TopicId == topic.TopicId
                            group attempt by quiz.AttemptsAllowed into g
                            where g.Max(x => x.AttemptCount) == g.Key
                            select g.Key
                        ).Count() == 1,
                        isPassed = (
                            from q in this._lXPDbContext.Quizzes
                            join la in this._lXPDbContext.LearnerAttempts
                                on q.QuizId equals la.QuizId
                            where la.LearnerId == learnerId && q.TopicId == topic.TopicId
                            group la by new { la.QuizId, q.PassMark } into g
                            where g.Max(x => x.Score) >= g.Key.PassMark
                            select g.Key.PassMark
                        ).Count() > 0,

                        Materials = (
                            from material in this._lXPDbContext.Materials
                            join materialType in this._lXPDbContext.MaterialTypes
                                on material.MaterialTypeId equals materialType.MaterialTypeId

                            where material.TopicId == topic.TopicId && material.IsActive
                            select new
                            {
                                material.MaterialId,
                                MaterialName = material.Name,
                                MaterialType = materialType.Type,
                                isCompleted = (
                                    from lp in this._lXPDbContext.LearnerProgresses
                                    join m in this._lXPDbContext.Materials
                                        on lp.Material.MaterialId equals m.MaterialId
                                    where
                                        lp.Material.MaterialId == material.MaterialId
                                        && lp.Learner.LearnerId == learnerId
                                    select new { lp, m }
                                )
                                    .Select(x =>
                                        x.lp.WatchTime.ToTimeSpan()
                                            / x.m.Duration.ToTimeSpan()
                                            * 100
                                        > 80
                                    )
                                    .FirstOrDefault(),
                                Material = string.Format(
                                    "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                                    this._contextAccessor.HttpContext.Request.Scheme,
                                    this._contextAccessor.HttpContext.Request.Host,
                                    this._contextAccessor.HttpContext.Request.PathBase,
                                    material.FilePath
                                ),
                                MaterialDuration = material.Duration
                            }
                        ).ToList(),
                    }
                ).ToList()
            };

        // Convert the result to a list so that it can be enumerated multiple times
        var resultList = result.ToList();

        // Update the IsWatched field for each material where isCompleted is true
        foreach (var material in resultList.SelectMany(r => r.Topics).SelectMany(t => t.Materials))
        {
            if (material.isCompleted)
            {
                var lp = this._lXPDbContext.LearnerProgresses.First(lp =>
                    lp.Material.MaterialId == material.MaterialId
                    && lp.Learner.LearnerId == learnerId
                );
                lp.IsWatched = 1;
            }
        }

        this._lXPDbContext.SaveChanges();

        return resultList;
    }
}


//        from enrollment in _lXPDbContext.Enrollments
//        where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
//        select new
//            enrollmentid = enrollment.EnrollmentId,
//            enrolledCourseId = enrollment.CourseId,
//            enrolledCoursename = enrollment.Course.Title,
//            enrolledcoursedescription = enrollment.Course.Description,
//            enrolledcoursecategory = enrollment.Course.Category.Category,
//            enrolledcourselevels = enrollment.Course.Level.Level,
//            Thumbnailimage = String.Format(
//                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
//                _contextAccessor.HttpContext.Request.Scheme,
//                _contextAccessor.HttpContext.Request.Host,
//                _contextAccessor.HttpContext.Request.PathBase,
//                enrollment.Course.Thumbnail
//            ),

//            Topics = (
//                from topic in _lXPDbContext.Topics
//                where topic.CourseId == enrollment.CourseId && topic.IsActive == true
//                orderby topic.CreatedAt ascending
//                select new
//                    TopicName = topic.Name,
//                    TopicDescription = topic.Description,
//                    TopicId = topic.TopicId,
//                    TopicIsActive = topic.IsActive,
//                    IsQuiz = _lXPDbContext.Quizzes.Any(quizzes =>
//                        quizzes.TopicId == topic.TopicId
//                    )
//                        ? (
//                            from q in _lXPDbContext.Quizzes
//                            join la in _lXPDbContext.LearnerAttempts
//                                on q.QuizId equals la.QuizId
//                            where la.LearnerId == learnerId && q.TopicId == topic.TopicId
//                            group la by new { la.QuizId, q.PassMark } into g
//                            where g.Max(x => x.Score) >= g.Key.PassMark
//                            select g.Key.PassMark
//                        ).Count() == 0
//                        : false,
//                    IsFeedBack = _lXPDbContext.Topicfeedbackquestions.Any(
//                        topicfeedbackquesion =>
//                            topicfeedbackquesion.TopicId == topic.TopicId
//                    )
//                        ? (
//                            from tfq in _lXPDbContext.Topicfeedbackquestions
//                            join fr in _lXPDbContext.Feedbackresponses
//                                on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
//                            where fr.LearnerId == learnerId && tfq.TopicId == topic.TopicId
//                            select tfq
//                        ).Count() == 0
//                        : false,
//                    isAttemptOver = (
//                        from quiz in _lXPDbContext.Quizzes
//                        join attempt in _lXPDbContext.LearnerAttempts
//                            on quiz.QuizId equals attempt.QuizId
//                        where
//                            attempt.LearnerId == learnerId && quiz.TopicId == topic.TopicId
//                        group attempt by quiz.AttemptsAllowed into g
//                        where g.Max(x => x.AttemptCount) == g.Key
//                        select g.Key
//                    ).Count() == 1,
//                    Materials = (
//                        from material in _lXPDbContext.Materials
//                        join materialType in _lXPDbContext.MaterialTypes
//                            on material.MaterialTypeId equals materialType.MaterialTypeId

//                        where material.TopicId == topic.TopicId && material.IsActive == true
//                        select new
//                            MaterialId = material.MaterialId,
//                            MaterialName = material.Name,
//                            MaterialType = materialType.Type,
//                            Material = String.Format(
//                                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
//                                _contextAccessor.HttpContext.Request.Scheme,
//                                _contextAccessor.HttpContext.Request.Host,
//                                _contextAccessor.HttpContext.Request.PathBase,
//                                material.FilePath
//                            ),
//                            MaterialDuration = material.Duration
//                    ).ToList(),
//            ).ToList()


//        from enrollment in _lXPDbContext.Enrollments
//        where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
//        select new
//            enrollmentid = enrollment.EnrollmentId,
//            enrolledCourseId = enrollment.CourseId,
//            enrolledCoursename = enrollment.Course.Title,
//            enrolledcoursedescription = enrollment.Course.Description,
//            enrolledcoursecategory = enrollment.Course.Category.Category,
//            enrolledcourselevels = enrollment.Course.Level.Level,
//            Thumbnailimage = String.Format(
//                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
//                _contextAccessor.HttpContext.Request.Scheme,
//                _contextAccessor.HttpContext.Request.Host,
//                _contextAccessor.HttpContext.Request.PathBase,
//                enrollment.Course.Thumbnail
//            ),

//            Topics = (
//                from topic in _lXPDbContext.Topics
//                where topic.CourseId == enrollment.CourseId && topic.IsActive == true
//                select new
//                    TopicName = topic.Name,
//                    TopicDescription = topic.Description,
//                    TopicId = topic.TopicId,
//                    TopicIsActive = topic.IsActive,
//                    Materials = (
//                        from material in _lXPDbContext.Materials
//                        join materialType in _lXPDbContext.MaterialTypes
//                            on material.MaterialTypeId equals materialType.MaterialTypeId

//                        where material.TopicId == topic.TopicId && material.IsActive == true
//                        select new
//                            MaterialId = material.MaterialId,
//                            MaterialName = material.Name,
//                            MaterialType = materialType.Type,
//                            Material = String.Format(
//                                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
//                                _contextAccessor.HttpContext.Request.Scheme,
//                                _contextAccessor.HttpContext.Request.Host,
//                                _contextAccessor.HttpContext.Request.PathBase,
//                                material.FilePath
//                            ),
//                            MaterialDuration = material.Duration
//                    ).ToList(),
//                    //MaterialType =(from materialType in _lXPDbContext.MaterialTypes select new
//                    //{
//                    //    MaterialType=materialType.Type,
//                    //    MaterialTypeId=materialType.MaterialTypeId,

//                    //}).ToList(),
//            ).ToList()
//         from enrollment in _lXPDbContext.Enrollments
//         where enrollment.LearnerId == learnerId && enrollment.CourseId == courseId
//         select new
//             enrollmentid = enrollment.EnrollmentId,
//             enrolledCourseId = enrollment.CourseId,
//             enrolledCoursename = enrollment.Course.Title,
//             enrolledcoursedescription = enrollment.Course.Description,
//             enrolledcoursecategory = enrollment.Course.Category.Category,
//             enrolledcourselevels = enrollment.Course.Level.Level,
//             Thumbnailimage = String.Format(
//                 "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
//                 _contextAccessor.HttpContext.Request.Scheme,
//                 _contextAccessor.HttpContext.Request.Host,
//                 _contextAccessor.HttpContext.Request.PathBase,
//                 enrollment.Course.Thumbnail
//             ),

//             Topics = (
//                 from topic in _lXPDbContext.Topics
//                 where topic.CourseId == enrollment.CourseId && topic.IsActive == true
//                 select new
//                     TopicName = topic.Name,
//                     TopicDescription = topic.Description,
//                     TopicId = topic.TopicId,
//                     TopicIsActive = topic.IsActive,
//                     IsQuiz = _lXPDbContext.Quizzes.Any(quizzes =>
//                         quizzes.TopicId == topic.TopicId
//                     )
//                         ? (
//                             from q in _lXPDbContext.Quizzes
//                             join la in _lXPDbContext.LearnerAttempts
//                                 on q.QuizId equals la.QuizId
//                             where la.LearnerId == learnerId && q.TopicId == topic.TopicId
//                             group la by new { la.QuizId, q.PassMark } into g
//                             where g.Max(x => x.Score) >= g.Key.PassMark
//                             select g.Key.PassMark
//                         ).Count() != 0
//                         : true,
//                     IsFeedBack = _lXPDbContext.Topicfeedbackquestions.Any(
//                         topicfeedbackquesion =>
//                             topicfeedbackquesion.TopicId == topic.TopicId
//                     )
//                         ? (
//                             from tfq in _lXPDbContext.Topicfeedbackquestions
//                             join fr in _lXPDbContext.Feedbackresponses
//                                 on tfq.TopicFeedbackQuestionId equals fr.TopicFeedbackQuestionId
//                             where fr.LearnerId == learnerId && tfq.TopicId == topic.TopicId
//                             select tfq
//                         ).Count() == 0
//                         : true,
//                     Materials = (
//                         from material in _lXPDbContext.Materials
//                         join materialType in _lXPDbContext.MaterialTypes
//                             on material.MaterialTypeId equals materialType.MaterialTypeId

//                         where material.TopicId == topic.TopicId && material.IsActive == true
//                         select new
//                             MaterialId = material.MaterialId,
//                             MaterialName = material.Name,
//                             MaterialType = materialType.Type,
//                             Material = String.Format(
//                                 "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
//                                 _contextAccessor.HttpContext.Request.Scheme,
//                                 _contextAccessor.HttpContext.Request.Host,
//                                 _contextAccessor.HttpContext.Request.PathBase,
//                                 material.FilePath
//                             ),
//                             MaterialDuration = material.Duration
//                     ).ToList(),
//                     //MaterialType =(from materialType in _lXPDbContext.MaterialTypes select new
//                     //{
//                     //    MaterialType=materialType.Type,
//                     //    MaterialTypeId=materialType.MaterialTypeId,

//                     //}).ToList(),
//             ).ToList()
