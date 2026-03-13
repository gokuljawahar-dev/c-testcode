namespace LXP.Data.IRepository;

using LXP.Common.Entities;
using LXP.Common.ViewModels;

public interface ICourseRepository
{
    void AddCourse(Course course);
    bool AnyCourseByCourseTitle(string courseTitle);
    Course GetCourseDetailsByCourseName(string courseName);

    Course GetCourseDetailsByCourseId(Guid CourseId);

    Course FindCourseid(Guid courseid);

    public Enrollment FindEntrollmentcourse(Guid Courseid);

    Task Deletecourse(Course course);

    Task Changecoursestatus(Course course);

    Task Updatecourse(Course course);

    IEnumerable<CourseDetailsViewModel> GetAllCourse();
    IEnumerable<CourseDetailsViewModel> GetLimitedCourse();
    IEnumerable<CourseListViewModel> GetAllCourseDetails();

    IEnumerable<CourseRatingViewModel> GetCourseRating();

    IEnumerable<TopicRatingViewModel> GetTopicRating();

    Task<dynamic> GetAllCourseDetailsByLearnerId(Guid learnerId);
}
