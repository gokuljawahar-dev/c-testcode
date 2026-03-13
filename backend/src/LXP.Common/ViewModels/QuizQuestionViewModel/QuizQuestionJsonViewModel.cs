namespace LXP.Common.ViewModels.QuizQuestionViewModel;

public class QuizQuestionJsonViewModel
{
    public int QuestionNumber { get; set; }
    public string QuestionType { get; set; }
    public string Question { get; set; }
    public string[] Options { get; set; }
    public string[] CorrectOptions { get; set; }
}
