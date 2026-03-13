namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface ICourseTopicRepository
{
    object GetTopicDetails(string courseId);

    // IEnumerable<TopicRatingViewModel> GetTopicRating();
    List<Topic> GetAllTopics();
    Task AddCourseTopic(Topic topic);
    bool AnyTopicByTopicName(string topicName);
    Task<Topic> GetTopicDetailsByTopicNameAndCourse(string topicName, Guid courseId);
    bool AnyTopicByTopicNameAndCourseId(string topicName, Guid courseId);
    Task<int> UpdateCourseTopic(Topic topic);
    object GetAllTopicDetailsByCourseId(string courseId);
    Task<Topic> GetTopicByTopicId(Guid topicId);
    Task<List<Topic>> GetTopicsbycouresId(Guid courseId);
    Task<List<LearnerProgress>> GetTopicsbyLearnerId(Guid courseId, Guid LearnerId);
}
