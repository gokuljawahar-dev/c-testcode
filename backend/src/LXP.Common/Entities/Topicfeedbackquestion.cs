namespace LXP.Common.Entities;

public partial class TopicFeedbackQuestion
{
    public Guid TopicFeedbackQuestionId { get; set; }

    public Guid TopicId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<FeedbackQuestionsOption> FeedbackQuestionsOptions { get; set; } =
        new List<FeedbackQuestionsOption>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } =
        new List<FeedbackResponse>();

    public virtual Topic Topic { get; set; } = null!;
}
