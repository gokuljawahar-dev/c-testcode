namespace LXP.Core.Services;

using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class CourseServices : ICourseServices
{
    private readonly ICourseRepository _courseRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICourseLevelRepository _courseLevelRepository;

    public CourseServices(
        ICourseRepository courseRepository,
        ICategoryRepository categoryRepository,
        ICourseLevelRepository courseLevelRepository,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._courseRepository = courseRepository;
        ;
        this._environment = environment;
        this._courseLevelRepository = courseLevelRepository;
        this._categoryRepository = categoryRepository;

        this._contextAccessor = httpContextAccessor;
    }

    public CourseListViewModel AddCourse(CourseViewModel course)
    {
        var isCourseExists = this._courseRepository.AnyCourseByCourseTitle(course.Title);

        if (!isCourseExists)
        {
            var levelId = Guid.Parse(course.Level);
            var level = this._courseLevelRepository.GetCourseLevelByCourseLevelId(levelId);
            var categoryId = Guid.Parse(course.Category);
            var category = this._categoryRepository.GetCategoryByCategoryId(categoryId);

            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{course.Thumbnailimage.FileName}";

            // Save the image to a designated folder (e.g., wwwroot/images)
            var uploadsFolder = Path.Combine(
                this._environment.WebRootPath,
                "CourseThumbnailImages"
            ); // Use WebRootPath
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                course.Thumbnailimage.CopyTo(stream); // Use await
            }

            var newCourse = new Course
            {
                CourseId = Guid.NewGuid(),
                Category = category,
                Level = level,
                Title = course.Title,
                Description = course.Description,
                Duration = course.Duration,
                Thumbnail = uniqueFileName,
                CreatedBy = course.CreatedBy,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsAvailable = true,
                ModifiedAt = null,
                ModifiedBy = null
            };
            this._courseRepository.AddCourse(newCourse);

            return this.GetCourseDetailsByCourseName(newCourse.Title);
        }
        else
        {
            return null;
        }
    }

    public async Task<CourseListDetailsViewModel> GetCourseDetailsByCourseId(string courseId)
    {
        var course = this._courseRepository.GetCourseDetailsByCourseId(Guid.Parse(courseId));
        var courseRating = this._courseRepository.GetCourseRating().ToDictionary(cr => cr.CourseId);

        var courseDetails = new CourseListDetailsViewModel()
        {
            CourseId = course.CourseId,
            Title = course.Title,
            Description = course.Description,

            Category = course.Category.Category,
            Level = course.Level.Level,
            CategoryId = course.Category.CategoryId,
            LevelId = course.Level.LevelId,
            Duration = course.Duration,
            Thumbnail = string.Format(
                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                course.Thumbnail
            ),
            CreatedAt = course.CreatedAt,
            IsActive = course.IsActive,
            IsAvailable = course.IsAvailable,
            ModifiedAt = course.ModifiedAt,
            CreatedBy = course.CreatedBy,
            ModifiedBy = course.ModifiedBy,
            AverageRating = courseRating.TryGetValue(course.CourseId, out var value)
                ? value.Rating
                : 0
        };

        return courseDetails;
    }

    public CourseListViewModel GetCourseDetailsByCourseName(string courseName)
    {
        var course = this._courseRepository.GetCourseDetailsByCourseName(courseName);
        var courseDetails = new CourseListViewModel
        {
            CourseId = course.CourseId,
            Title = course.Title,
            Description = course.Description,
            Category = course.Category.Category,
            Level = course.Level.Level,
            Duration = course.Duration,
            Thumbnail = string.Format(
                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                course.Thumbnail
            ),
            CreatedAt = course.CreatedAt,
            IsActive = course.IsActive,
            IsAvailable = course.IsAvailable,
            ModifiedAt = course.ModifiedAt,
            CreatedBy = course.CreatedBy,
            ModifiedBy = course.ModifiedBy,
        };
        return courseDetails;
    }

    public Course GetCourseByCourseId(Guid courseId)
    {
        var course = this._courseRepository.GetCourseDetailsByCourseId(courseId);

        var courseView = new Course
        {
            CourseId = courseId,
            LevelId = course.LevelId,
            CategoryId = course.CategoryId,
            Title = course.Title,
            Description = course.Description,
            Duration = course.Duration,
            Thumbnail = string.Format(
                "{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                course.Thumbnail
            )
        };
        return courseView;
    }

    public async Task<bool> Deletecourse(Guid courseId)
    {
        var Course = this._courseRepository.FindCourseid(courseId);
        if (Course != null)
        {
            var Enrollentcourse = this._courseRepository.FindEntrollmentcourse(courseId);
            if (Enrollentcourse == null)
            {
                this._courseRepository.Deletecourse(Course);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> Changecoursestatus(Coursestatus status)
    {
        var course = this._courseRepository.FindCourseid(status.CourseId);
        if (course != null)
        {
            course.IsAvailable = status.IsAvailable;
            course.ModifiedAt = DateTime.Now;
            await this._courseRepository.Changecoursestatus(course);
            return true;
        }
        return false;
    }

    public async Task<bool> Updatecourse(CourseUpdateModel course)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{course.Thumbnailimage.FileName}";
        var uploadsFolder = Path.Combine(this._environment.WebRootPath, "CourseThumbnailImages"); // Use WebRootPath
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            course.Thumbnailimage.CopyTo(stream);
        }
        var individualcourse = this._courseRepository.FindCourseid(course.CourseId);
        if (individualcourse != null)
        {
            individualcourse!.Title = course.Title;
            individualcourse.CategoryId = course.CategoryId;
            individualcourse.LevelId = course.LevelId;
            individualcourse.Description = course.Description;
            individualcourse.Duration = course.Duration;
            individualcourse.Thumbnail = uniqueFileName;
            individualcourse.ModifiedBy = course.ModifiedBy;
            individualcourse.ModifiedAt = DateTime.Now;
            await this._courseRepository.Updatecourse(individualcourse);
            return true;
        }
        return false;
    }

    public IEnumerable<CourseDetailsViewModel> GetAllCourse() =>
        this._courseRepository.GetAllCourse();

    public IEnumerable<CourseDetailsViewModel> GetLimitedCourse() =>
        this._courseRepository.GetLimitedCourse();

    public IEnumerable<CourseListViewModel> GetAllCourseDetails() =>
        this._courseRepository.GetAllCourseDetails();


    public IEnumerable<TopicRatingViewModel> GetTopicRating() =>
        this._courseRepository.GetTopicRating();

    public async Task<dynamic> GetAllCourseDetailsByLearnerId(string learnerId)
    {
        var LearnerId = Guid.Parse(learnerId);
        var Courses = this._courseRepository.GetAllCourseDetailsByLearnerId(LearnerId);
        return Courses;
    }
}
