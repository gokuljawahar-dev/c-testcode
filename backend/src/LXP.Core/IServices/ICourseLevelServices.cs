namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface ICourseLevelServices
{
    Task<List<CourseLevelListViewModel>> GetAllCourseLevel(string CreatedBy);
    Task AddCourseLevel(string Level, string CreatedBy);
}
