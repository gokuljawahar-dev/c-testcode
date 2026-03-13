namespace LXP.Common.ViewModels.TopicFeedbackQuestionViewModel;

public class TopicFeedbackQuestionViewModel
{
    public Guid TopicId { get; set; }
    public string Question { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public List<TopicFeedbackQuestionsOptionViewModel> Options { get; set; } = [];
}
