// namespace LXP.Common.ViewModels.QuizViewModel

namespace LXP.Common.ViewModels.QuizViewModel;

public class QuizViewModel
{
    public Guid QuizId { get; set; }
    public Guid CourseId { get; set; }
    public Guid TopicId { get; set; }
    public string NameOfQuiz { get; set; } = null!;
    public int Duration { get; set; }
    public int? AttemptsAllowed { get; set; }
    public int PassMark { get; set; }
}
