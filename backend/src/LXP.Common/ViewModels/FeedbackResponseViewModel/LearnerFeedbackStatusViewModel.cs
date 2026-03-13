namespace LXP.Common.ViewModels.FeedbackResponseViewModel;

public class LearnerFeedbackStatusViewModel
{
    public Guid LearnerId { get; set; }
    public bool IsQuizFeedbackSubmitted { get; set; }
    public bool IsTopicFeedbackSubmitted { get; set; }
    public bool IsCourseFeedbackSubmitted { get; set; }
}
