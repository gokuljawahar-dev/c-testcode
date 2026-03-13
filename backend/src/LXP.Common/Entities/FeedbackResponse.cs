namespace LXP.Common.Entities;

public partial class FeedbackResponse
{
    public Guid FeedbackResponseId { get; set; }

    public Guid? QuizFeedbackQuestionId { get; set; }

    public Guid? TopicFeedbackQuestionId { get; set; }

    public Guid? CourseFeedbackQuestionId { get; set; }

    public Guid LearnerId { get; set; }

    public string? Response { get; set; }

    public Guid? OptionId { get; set; }

    public string? GeneratedBy { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual CourseFeedbackQuestion? CourseFeedbackQuestion { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual QuizFeedbackQuestion? QuizFeedbackQuestion { get; set; }

    public virtual TopicFeedbackQuestion? TopicFeedbackQuestion { get; set; }
}
