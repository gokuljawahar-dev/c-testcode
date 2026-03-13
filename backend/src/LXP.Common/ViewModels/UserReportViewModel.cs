namespace LXP.Common.ViewModels;

public class UserReportViewModel
{
    public string UserName { get; set; }
    public string LearnerId { get; set; }
    public int EnrolledCourse { get; set; }
    public int CompletedCourse { get; set; }
    public DateTime LastLogin { get; set; }
}
