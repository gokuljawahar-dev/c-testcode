namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface ICategoryRepository
{
    Task AddCategory(CourseCategory category);

    Task<List<CourseCategory>> GetAllCategory();

    Task<bool> AnyCategoryByCategoryName(string Category);
    CourseCategory GetCategoryByCategoryId(Guid categoryId);
    CourseCategory GetCategoryByCategoryName(string categoryName);
}
