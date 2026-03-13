namespace LXP.Common.ViewModels;

public class QuizScorelearnerViewModel
{
    public Guid LearnerId { get; set; }
    public string? LearnerName { get; set; }
    public string? Profilephoto { get; set; }
    public int TotalNoofQuizAttempts { get; set; }
    public int LearnerAttempts { get; set; }
    public float Score { get; set; }
    public string EmailId { get; set; }
}
