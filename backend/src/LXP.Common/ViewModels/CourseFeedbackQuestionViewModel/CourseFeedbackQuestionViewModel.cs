namespace LXP.Common.ViewModels.CourseFeedbackQuestionViewModel;

public class CourseFeedbackQuestionViewModel
{
    public Guid CourseId { get; set; }
    public string Question { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public List<CourseFeedbackQuestionsOptionViewModel> Options { get; set; } = [];
}
