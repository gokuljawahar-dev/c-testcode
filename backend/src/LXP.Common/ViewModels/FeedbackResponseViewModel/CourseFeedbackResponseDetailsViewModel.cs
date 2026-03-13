namespace LXP.Common.ViewModels.FeedbackResponseViewModel;

public class CourseFeedbackResponseDetailsViewModel
{
    public Guid CourseFeedbackQuestionId { get; set; }
    public Guid LearnerId { get; set; }
    public string Question { get; set; }
    public string QuestionType { get; set; }
    public string Response { get; set; }
    public string OptionText { get; set; }
    public string LearnerName { get; set; } // Added property
    public string CourseName { get; set; } // Added property
}
