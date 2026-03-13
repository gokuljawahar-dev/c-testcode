namespace LXP.Common.Entities;

public partial class FeedbackQuestionsOption
{
    public Guid FeedbackQuestionOptionId { get; set; }

    public Guid? QuizFeedbackQuestionId { get; set; }

    public Guid? TopicFeedbackQuestionId { get; set; }

    public Guid? CourseFeedbackQuestionId { get; set; }

    public string OptionText { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual CourseFeedbackQuestion? CourseFeedbackQuestion { get; set; }

    public virtual QuizFeedbackQuestion? QuizFeedbackQuestion { get; set; }

    public virtual TopicFeedbackQuestion? TopicFeedbackQuestion { get; set; }
}
