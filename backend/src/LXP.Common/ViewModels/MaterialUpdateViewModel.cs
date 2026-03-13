namespace LXP.Common.ViewModels;

using Microsoft.AspNetCore.Http;

public class MaterialUpdateViewModel
{
    public string MaterialId { get; set; }

    public string Name { get; set; } = null!;

    public IFormFile Material { get; set; } = null!;

    public string? ModifiedBy { get; set; }
}
