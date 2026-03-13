namespace LXP.Core.Services;

using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class CourseLevelServices : ICourseLevelServices
{
    private readonly ICourseLevelRepository _courseLevelRepository;
    private readonly Mapper _levelMapper;

    public CourseLevelServices(ICourseLevelRepository courseLevelRepository)
    {
        this._courseLevelRepository = courseLevelRepository;
        var _configLevel = new MapperConfiguration(cfg =>
            cfg.CreateMap<CourseLevel, CourseLevelListViewModel>().ReverseMap()
        );
        this._levelMapper = new Mapper(_configLevel);
    }

    public async Task<List<CourseLevelListViewModel>> GetAllCourseLevel(string CreatedBy)
    {
        var CourseLevel = await this._courseLevelRepository.GetAllCourseLevel();
        if (CourseLevel.Count == 0)
        {
            await this.AddCourseLevel("Beginner", CreatedBy);
            await this.AddCourseLevel("Advanced", CreatedBy);
            await this.AddCourseLevel("Intermediate", CreatedBy);
        }
        return this._levelMapper.Map<List<CourseLevel>, List<CourseLevelListViewModel>>(
            await this._courseLevelRepository.GetAllCourseLevel()
        );
    }

    public async Task AddCourseLevel(string Level, string CreatedBy)
    {
        var course = new CourseLevel()
        {
            LevelId = Guid.NewGuid(),
            Level = Level,
            CreatedAt = DateTime.Now,
            CreatedBy = CreatedBy
        };
        await this._courseLevelRepository.AddCourseLevel(course);
    }
}
