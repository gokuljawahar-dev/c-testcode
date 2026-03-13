namespace LXP.Common.ViewModels;

public class LearnerProgressViewModel
{
    public Guid CourseId { get; set; }

    public Guid TopicId { get; set; }

    public Guid MaterialId { get; set; }

    public Guid LearnerId { get; set; }

    public TimeOnly WatchTime { get; set; }

}
