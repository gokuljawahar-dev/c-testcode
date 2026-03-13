namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface ICategoryServices
{
    Task<bool> AddCategory(CourseCategoryViewModel category);
    Task<List<CourseCategoryListViewModel>> GetAllCategory();
    Task<CourseCategoryListViewModel> GetCategoryByCategoryName(string categoryName);
}
