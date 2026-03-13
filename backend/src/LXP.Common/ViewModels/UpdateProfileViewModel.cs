//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace LXP.Common.ViewModels;

//namespace LXP.Common.ViewModels


using Microsoft.AspNetCore.Http;

public class UpdateProfileViewModel
{
    public string ProfileId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Dob { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public string Stream { get; set; } = null!;
    public IFormFile? ProfilePhoto { get; set; } // Make ProfilePhoto optional
}
