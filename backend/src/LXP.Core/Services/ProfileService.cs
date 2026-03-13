//using AutoMapper;
//using LXP.Common.Entities;
//using LXP.Common.ViewModels;
//using LXP.Core.IServices;
//using LXP.Data.IRepository;
//using LXP.Data.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace LXP.Core.Services;

//namespace LXP.Core.Services


//            this._profileRepository = profileRepository;
//            _learnerProfileMapper = new Mapper(_configCategory);


//            List<GetProfileViewModel> learnerProfile = _learnerProfileMapper.Map<List<LearnerProfile>, List<GetProfileViewModel>>(await _profileRepository.GetAllLearnerProfile());


using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly Mapper _learnerProfileMapper;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _contextAccessor;

    public ProfileService(
        IProfileRepository profileRepository,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._profileRepository = profileRepository;
        var _configCategory = new MapperConfiguration(cfg =>
            cfg.CreateMap<LearnerProfile, GetProfileViewModel>().ReverseMap()
        );
        this._learnerProfileMapper = new Mapper(_configCategory);
        this._environment = environment;
        this._contextAccessor = httpContextAccessor;
    }

    public async Task<List<GetProfileViewModel>> GetAllLearnerProfile()
    {
        var learnerProfile = this._learnerProfileMapper.Map<
            List<LearnerProfile>,
            List<GetProfileViewModel>
        >(await this._profileRepository.GetAllLearnerProfile());
        return learnerProfile;
    }

    public LearnerProfile GetLearnerProfileById(string id)
    {

        var profile = this._profileRepository.GetLearnerprofileDetailsByLearnerprofileId(
            Guid.Parse(id)
        );
        var profileIndividual = new LearnerProfile
        {
            ProfileId = profile.ProfileId,
            //ProfilePhoto = profile.ProfilePhoto,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Dob = profile.Dob,
            Gender = profile.Gender,
            Stream = profile.Stream,
            ContactNumber = profile.ContactNumber,
            ProfilePhoto = string.Format(
                "{0}://{1}{2}/wwwroot/LearnerProfileImages/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                profile.ProfilePhoto
            )
        };
        return profileIndividual;
    }

    public async Task UpdateProfile(UpdateProfileViewModel model)
    {
        var learnerProfile = this._profileRepository.GetLearnerprofileDetailsByLearnerprofileId(
            Guid.Parse(model.ProfileId)
        );

        if (model.ProfilePhoto != null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{model.ProfilePhoto.FileName}";
            var uploadsFolder = Path.Combine(this._environment.WebRootPath, "LearnerProfileImages"); // Use WebRootPath
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfilePhoto.CopyToAsync(stream);
            }

            learnerProfile.ProfilePhoto = uniqueFileName;
        }

        learnerProfile.FirstName = model.FirstName;
        learnerProfile.LastName = model.LastName;
        learnerProfile.ModifiedBy = $"{model.FirstName} {model.LastName}";
        learnerProfile.ModifiedAt = DateTime.Now;
        learnerProfile.ContactNumber = model.ContactNumber;
        learnerProfile.Dob = DateOnly.ParseExact(model.Dob, "yyyy-MM-dd", null);
        learnerProfile.Gender = model.Gender;
        learnerProfile.Stream = model.Stream;

        await this._profileRepository.UpdateProfile(learnerProfile);
    }

    public Guid GetprofileId(Guid learnerId) => this._profileRepository.GetProfileId(learnerId);
}
