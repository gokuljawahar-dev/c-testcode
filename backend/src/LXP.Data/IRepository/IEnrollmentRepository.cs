namespace LXP.Data.IRepository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;

public interface IEnrollmentRepository
{
    Task Addenroll(Enrollment enrollment);

    bool AnyEnrollmentByLearnerAndCourse(Guid learnerId, Guid courseId);

    object GetCourseandTopicsByLearnerId(Guid learnerId);
    public IEnumerable<EnrollmentReportViewModel> GetEnrollmentReport();
    public IEnumerable<EnrolledUserViewModel> GetEnrolledUser(Guid courseId);
    public IEnumerable<EnrollmentReportViewModel> GetEnrolledInprogressLearnerbyCourseId(
        Guid courseId
    );
    public IEnumerable<EnrollmentReportViewModel> GetEnrolledCompletedLearnerbyCourseId(
        Guid courseId
    );

    Enrollment FindEnrollmentId(Guid enrollmentId);
    Task DeleteEnrollment(Enrollment enrollment);
    object GetCourseandTopicsByCourseIdAndLearnerId(Guid courseId, Guid learnerId);

    Task UpdateCourseStarted(Enrollment enrollment);
}
