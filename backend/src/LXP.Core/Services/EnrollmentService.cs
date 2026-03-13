//using AutoMapper;
namespace LXP.Core.Services;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class EnrollmentService(
    IEnrollmentRepository enrollmentRepository,
    ILearnerRepository learnerRepository,
    ICourseRepository courseRepository,
    IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor
) : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository = enrollmentRepository;
    private readonly ILearnerRepository _learnerRepository = learnerRepository;
    private readonly ICourseRepository _courseRepository = courseRepository;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<bool> Addenroll(EnrollmentViewModel enrollment)
    {
        var isEnrolledExists = this._enrollmentRepository.AnyEnrollmentByLearnerAndCourse(
            enrollment.LearnerId,
            enrollment.CourseId
        );

        if (!isEnrolledExists)
        {
            var learner = this._learnerRepository.GetLearnerDetailsByLearnerId(
                enrollment.LearnerId
            );

            var course = this._courseRepository.GetCourseDetailsByCourseId(enrollment.CourseId);

            var newEnrollment = new Enrollment
            {
                EnrollmentId = Guid.NewGuid(),
                LearnerId = enrollment.LearnerId,
                CourseId = enrollment.CourseId,
                EnrollmentDate = DateTime.Now,
                EnrollStatus = true,
                CompletedStatus = 0,
                CreatedBy = "Admin",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                ModifiedBy = "Admin"
            };

            this._enrollmentRepository.Addenroll(newEnrollment);

            return true;
        }
        else
        {
            return false;
        }
    }

    public object GetCourseandTopicsByLearnerId(Guid learnerId) =>
        this._enrollmentRepository.GetCourseandTopicsByLearnerId(learnerId);

    public IEnumerable<EnrolledUserViewModel> GetEnrolledUsers(Guid courseId) =>
        this._enrollmentRepository.GetEnrolledUser(courseId);

    public IEnumerable<EnrollmentReportViewModel> GetEnrollmentsReport() =>
        this._enrollmentRepository.GetEnrollmentReport();

    public IEnumerable<EnrollmentReportViewModel> GetEnrolledCompletedLearnerbyCourseId(
        Guid courseId
    ) => this._enrollmentRepository.GetEnrolledCompletedLearnerbyCourseId(courseId);

    public IEnumerable<EnrollmentReportViewModel> GetEnrolledInprogressLearnerbyCourseId(
        Guid courseId
    ) => this._enrollmentRepository.GetEnrolledInprogressLearnerbyCourseId(courseId);

    public async Task<bool> DeleteEnrollment(Guid enrollmentId)
    {
        var enrollment = this._enrollmentRepository.FindEnrollmentId(enrollmentId);
        if (enrollment != null)
        {
            this._enrollmentRepository.DeleteEnrollment(enrollment);
            return true;
        }
        return false;
    }

    public object GetCourseandTopicsByCourseId(Guid courseId, Guid learnerId) =>
        this._enrollmentRepository.GetCourseandTopicsByCourseIdAndLearnerId(courseId, learnerId); //2106

    public async Task UpdateCourseStarted(Guid enrollmentId)
    {
        var enrollment = this._enrollmentRepository.FindEnrollmentId(enrollmentId);
        // Enrollment enrollment1=new Enrollment()
        enrollment.CourseStarted = true;
        await this._enrollmentRepository.UpdateCourseStarted(enrollment);
    }
}
