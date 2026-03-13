namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository(LXPDbContext lXPDbContext) : ICategoryRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;

    public async Task AddCategory(CourseCategory category)
    {
        await this._lXPDbContext.CourseCategories.AddAsync(category);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task<List<CourseCategory>> GetAllCategory() =>
        await this._lXPDbContext.CourseCategories.ToListAsync();

    public async Task<bool> AnyCategoryByCategoryName(string Category) =>
        await this._lXPDbContext.CourseCategories.AnyAsync(category =>
            category.Category == Category
        );

    public CourseCategory GetCategoryByCategoryId(Guid categoryId) =>
        this._lXPDbContext.CourseCategories.Find(categoryId);

    public CourseCategory GetCategoryByCategoryName(string categoryName) =>
        this._lXPDbContext.CourseCategories.First(category => category.Category == categoryName);
}
