namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface ICourseLevelRepository
{
    Task AddCourseLevel(CourseLevel level);

    Task<List<CourseLevel>> GetAllCourseLevel();
    CourseLevel GetCourseLevelByCourseLevelId(Guid courseLevelId);
}
