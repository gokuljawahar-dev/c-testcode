namespace LXP.Common.Entities;

public partial class CourseFeedbackQuestion
{
    public Guid CourseFeedbackQuestionId { get; set; }

    public Guid CourseId { get; set; }

    public int QuestionNo { get; set; }

    public string Question { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<FeedbackQuestionsOption> FeedbackQuestionsOptions { get; set; } =
        new List<FeedbackQuestionsOption>();

    public virtual ICollection<FeedbackResponse> FeedbackResponses { get; set; } =
        new List<FeedbackResponse>();
}
