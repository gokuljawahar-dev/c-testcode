namespace LXP.Core.IServices;

using LXP.Common.Entities;
using LXP.Common.ViewModels;

public interface ICourseServices
{
    Task<CourseListDetailsViewModel> GetCourseDetailsByCourseId(string courseId);
    Course GetCourseByCourseId(Guid courseId);

    CourseListViewModel GetCourseDetailsByCourseName(string courseName);
    CourseListViewModel AddCourse(CourseViewModel course);

    //Course GetCourseByCourseId(Guid courseId);

    Task<bool> Deletecourse(Guid courseId);

    Task<bool> Changecoursestatus(Coursestatus status);

    Task<bool> Updatecourse(CourseUpdateModel course);

    IEnumerable<CourseDetailsViewModel> GetAllCourse();
    IEnumerable<CourseDetailsViewModel> GetLimitedCourse();
    IEnumerable<CourseListViewModel> GetAllCourseDetails();

    //  IEnumerable<CourseRatingViewModel> GetCourseRating();


    IEnumerable<TopicRatingViewModel> GetTopicRating();

    Task<dynamic> GetAllCourseDetailsByLearnerId(string learnerId);
}
