namespace LXP.Common.ViewModels.QuizViewModel;

public class CreateQuizViewModel
{
    public string? NameOfQuiz { get; set; }
    public int Duration { get; set; }
    public int PassMark { get; set; }
    public int? AttemptsAllowed { get; set; }
    public Guid TopicId { get; set; }
}
