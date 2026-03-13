namespace LXP.Common.ViewModels.QuizEngineViewModel;

public class QuestionResponseViewModel
{
    public string? Question { get; set; }
    public string? QuestionType { get; set; }
    public Guid QuizQuestionId { get; set; }
    public List<string> SelectedOptions { get; set; } = [];
    public List<QuizEngineOptionViewModel>? Options { get; set; }
}
