namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface ICourseTopicServices
{
    object GetAllTopicDetailsByCourseId(string courseId);
    Task<CourseTopicListViewModel> AddCourseTopic(CourseTopicViewModel courseTopic);
    bool UpdateCourseTopic(CourseTopicUpdateModel courseTopic);
    Task<bool> SoftDeleteTopic(string topicId);
    Task<CourseTopicListViewModel> GetTopicDetailsByTopicNameAndCourseId(
        string topicName,
        string courseId
    );
    Task<CourseTopicListViewModel> GetTopicDetailsByTopicId(string topicId);

    object GetTopicDetails(string courseId);
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LXP.Common.Entities;
//using LXP.Common.ViewModels;

//namespace LXP.Core.IServices
//        object GetAllTopicDetailsByCourseId(string courseId);
//        Task<CourseTopicListViewModel> AddCourseTopic(CourseTopicViewModel courseTopic);
//        Task<bool> UpdateCourseTopic(CourseTopicUpdateModel courseTopic);
//        Task<bool> SoftDeleteTopic(string topicId);
//        Task<CourseTopicListViewModel> GetTopicDetailsByTopicNameAndCourseId(
//            string topicName,
//            string courseId
//        );
//        Task<CourseTopicListViewModel> GetTopicDetailsByTopicId(string topicId);

//        object GetTopicDetails(string courseId);
