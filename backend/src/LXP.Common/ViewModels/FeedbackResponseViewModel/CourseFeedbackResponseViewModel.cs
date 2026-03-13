namespace LXP.Common.ViewModels.FeedbackResponseViewModel;

public class CourseFeedbackResponseViewModel
{
    public Guid CourseFeedbackQuestionId { get; set; }
    public Guid CourseId { get; set; }
    public Guid LearnerId { get; set; }
    public string? Response { get; set; }
    public string? OptionText { get; set; }
}
