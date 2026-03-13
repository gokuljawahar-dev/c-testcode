namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class CourseLevelRepository(LXPDbContext lXPDbContext) : ICourseLevelRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;

    public async Task AddCourseLevel(CourseLevel level)
    {
        await this._lXPDbContext.CourseLevels.AddAsync(level);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task<List<CourseLevel>> GetAllCourseLevel() =>
        this._lXPDbContext.CourseLevels.ToList();

    public CourseLevel GetCourseLevelByCourseLevelId(Guid courseLevelId) =>
        this._lXPDbContext.CourseLevels.Find(courseLevelId);
}
