namespace LXP.Common.ViewModels;

using Microsoft.AspNetCore.Http;

public class MaterialViewModel
{
    public string TopicId { get; set; }

    public string MaterialTypeId { get; set; }

    public string Name { get; set; }

    public IFormFile Material { get; set; }

    public TimeOnly Duration { get; set; }

    public string CreatedBy { get; set; }
}
