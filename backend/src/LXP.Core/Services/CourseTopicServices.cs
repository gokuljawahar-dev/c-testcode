namespace LXP.Core.Services;

using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class CourseTopicServices : ICourseTopicServices
{
    private readonly ICourseTopicRepository _courseTopicRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly Mapper _courseTopicMapper;

    public CourseTopicServices(
        ICourseTopicRepository courseTopicRepository,
        ICourseRepository courseRepository
    )
    {
        var _configCourseTopic = new MapperConfiguration(cfg =>
            cfg.CreateMap<Topic, CourseTopicListViewModel>().ReverseMap()
        );
        this._courseTopicMapper = new Mapper(_configCourseTopic);

        this._courseTopicRepository = courseTopicRepository;
        this._courseRepository = courseRepository;
    }

    public object GetAllTopicDetailsByCourseId(string courseId) =>
        this._courseTopicRepository.GetAllTopicDetailsByCourseId(courseId);

    public async Task<CourseTopicListViewModel> GetTopicDetailsByTopicNameAndCourseId(
        string topicName,
        string courseId
    )
    {
        //Course course = _courseRepository.GetCourseDetailsByCourseId(Guid.Parse(courseId));
        var topic = await this._courseTopicRepository.GetTopicDetailsByTopicNameAndCourse(
            topicName,
            Guid.Parse(courseId)
        );
        var courseTopic = new CourseTopicListViewModel()
        {
            TopicId = topic.TopicId,
            CourseId = topic.CourseId,
            Name = topic.Name,
            Description = topic.Description,
            IsActive = topic.IsActive,
            CreatedAt = topic.CreatedAt,
            CreatedBy = topic.CreatedBy,
            ModifiedAt = topic.ModifiedAt,
            ModifiedBy = topic.ModifiedBy
        };
        return courseTopic;
    }

    public async Task<CourseTopicListViewModel> GetTopicDetailsByTopicId(string topicId)
    {
        var topic = await this._courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
        var courseTopic = new CourseTopicListViewModel()
        {
            TopicId = topic.TopicId,
            CourseId = topic.CourseId,
            Name = topic.Name,
            Description = topic.Description,
            IsActive = topic.IsActive,
            CreatedAt = topic.CreatedAt,
            CreatedBy = topic.CreatedBy,
            ModifiedAt = topic.ModifiedAt,
            ModifiedBy = topic.ModifiedBy
        };
        return courseTopic;
    }

    public object GetTopicDetails(string courseId) =>
        this._courseTopicRepository.GetTopicDetails(courseId);

    public async Task<CourseTopicListViewModel> AddCourseTopic(CourseTopicViewModel courseTopic)
    {
        var isTopicExists = this._courseTopicRepository.AnyTopicByTopicNameAndCourseId(
            courseTopic.Name,
            Guid.Parse(courseTopic.CourseId)
        );
        var courseId = Guid.Parse(courseTopic.CourseId);
        var course = this._courseRepository.GetCourseDetailsByCourseId(courseId);
        if (!isTopicExists)
        {
            var topic = new Topic()
            {
                TopicId = Guid.NewGuid(),
                Name = courseTopic.Name,
                Description = courseTopic.Description,
                CourseId = course.CourseId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = courseTopic.CreatedBy,
                ModifiedAt = null,
                ModifiedBy = null
            };
            this._courseTopicRepository.AddCourseTopic(topic);

            return this._courseTopicMapper.Map<Topic, CourseTopicListViewModel>(topic);
            ;
        }
        else
        {
            return null;
        }
    }

    //    Topic topic = await _courseTopicRepository.GetTopicByTopicId(
    //        Guid.Parse(courseTopic.TopicId)
    //    );
    //    //List<Topic> topicsListByCourseId = await _courseTopicRepository.GetTopicsbycouresId(
    //    //    topic.CourseId
    //    //);
    //    //topicsListByCourseId.Remove(topic);
    //    //bool isTopicAlreadyExists = topicsListByCourseId.Any(topics =>
    //    //    topics.Name == courseTopic.Name
    //    //);
    //    //if (!isTopicAlreadyExists)
    //    //{
    //    topic.Name = courseTopic.Name;
    //    topic.Description = courseTopic.Description;
    //    topic.ModifiedBy = courseTopic.ModifiedBy;
    //    topic.ModifiedAt = DateTime.Now;
    //    //}
    //    //else
    //    //{
    //    //    return false;
    //    //}
    public bool UpdateCourseTopic(CourseTopicUpdateModel courseTopic)
    {
        //Topic topic = await _courseTopicRepository.GetTopicByTopicId(
        //    Guid.Parse(courseTopic.TopicId)
        //);
        var topicsList = this._courseTopicRepository.GetAllTopics();
        Topic topic = topicsList.FirstOrDefault(topic =>
            topic.TopicId == Guid.Parse(courseTopic.TopicId)
        );
        var topicsListByCourseId = topicsList
            .Where(topics => topics.CourseId == topic.CourseId)
            .ToList();
        topicsListByCourseId.Remove(topic);
        var isTopicAlreadyExists = topicsListByCourseId.Any(topics =>
            topics.Name == courseTopic.Name
        );
        if (!isTopicAlreadyExists)
        {
            topic.Name = courseTopic.Name;
            topic.Description = courseTopic.Description;
            topic.ModifiedBy = courseTopic.ModifiedBy;
            topic.ModifiedAt = DateTime.Now;
            this._courseTopicRepository.UpdateCourseTopic(topic);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> SoftDeleteTopic(string topicId)
    {
        var topic = await this._courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
        topic.IsActive = false;
        var isTopicDeleted = await this._courseTopicRepository.UpdateCourseTopic(topic) > 0;
        if (isTopicDeleted)
        {
            return true;
        }
        return false;
    }
}
