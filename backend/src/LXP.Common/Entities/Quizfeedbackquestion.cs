namespace LXP.Common.Entities;

public partial class QuizFeedbackQuestion
{
    public Guid QuizFeedbackQuestionId { get; set; }

    public Guid QuizId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<FeedbackQuestionsOption> FeedbackQuestionsOptions { get; set; } =
        new List<FeedbackQuestionsOption>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } =
        new List<FeedbackResponse>();

    public virtual Quiz Quiz { get; set; } = null!;
}
