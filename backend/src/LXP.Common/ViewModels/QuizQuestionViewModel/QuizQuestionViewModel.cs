namespace LXP.Common.ViewModels.QuizQuestionViewModel;

public class QuizQuestionViewModel
{
    public Guid QuizId { get; set; }

    public string Question { get; set; } = null!;
    public string QuestionType { get; set; } = null!;

    public List<QuestionOptionViewModel> Options { get; set; } = [];
}
