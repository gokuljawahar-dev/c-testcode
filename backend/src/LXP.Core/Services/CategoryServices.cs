namespace LXP.Core.Services;

using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class CategoryServices : ICategoryServices
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly Mapper _categoryMapper;

    public CategoryServices(ICategoryRepository categoryRepository)
    {
        this._categoryRepository = categoryRepository;
        var _configCategory = new MapperConfiguration(cfg =>
            cfg.CreateMap<CourseCategory, CourseCategoryListViewModel>().ReverseMap()
        );
        this._categoryMapper = new Mapper(_configCategory);
    }

    public async Task<bool> AddCategory(CourseCategoryViewModel category)
    {
        var isCategoryExists = await this._categoryRepository.AnyCategoryByCategoryName(
            category.Category
        );
        if (!isCategoryExists)
        {
            var courseCategory = new CourseCategory()
            {
                CategoryId = Guid.NewGuid(),
                Category = category.Category,
                CreatedAt = DateTime.Now,
                CreatedBy = category.CreatedBy
            };
            await this._categoryRepository.AddCategory(courseCategory);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<List<CourseCategoryListViewModel>> GetAllCategory()
    {
        var category = this._categoryMapper.Map<
            List<CourseCategory>,
            List<CourseCategoryListViewModel>
        >(await this._categoryRepository.GetAllCategory());
        return category;
    }

    public Task<CourseCategoryListViewModel> GetCategoryByCategoryName(string categoryName)
    {
        var category = this._categoryMapper.Map<CourseCategory, CourseCategoryListViewModel>(
            this._categoryRepository.GetCategoryByCategoryName(categoryName)
        );
        return Task.FromResult(category);
    }
}
