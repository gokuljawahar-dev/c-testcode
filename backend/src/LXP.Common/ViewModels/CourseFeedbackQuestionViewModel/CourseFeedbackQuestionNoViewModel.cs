namespace LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;

public class CourseFeedbackQuestionNoViewModel
{
    public Guid CourseFeedbackQuestionId { get; set; }
    public Guid CourseId { get; set; }
    public int QuestionNo { get; set; }
    public string Question { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public List<CourseFeedbackQuestionsOptionViewModel> Options { get; set; } = [];
}
