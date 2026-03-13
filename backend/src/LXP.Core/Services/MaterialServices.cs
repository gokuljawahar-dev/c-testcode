namespace LXP.Core.Services;

using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class MaterialServices : IMaterialServices
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICourseTopicRepository _courseTopicRepository;
    private readonly IMaterialTypeRepository _materialTypeRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly Mapper _courseMaterialMapper;

    public MaterialServices(
        IMaterialTypeRepository materialTypeRepository,
        IMaterialRepository materialRepository,
        ICourseTopicRepository courseTopicRepository,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._materialRepository = materialRepository;
        this._courseTopicRepository = courseTopicRepository;
        this._materialTypeRepository = materialTypeRepository;
        this._environment = environment;
        this._contextAccessor = httpContextAccessor;
        var _configCourseMaterial = new MapperConfiguration(cfg =>
            cfg.CreateMap<Material, MaterialListViewModel>().ReverseMap()
        );
        this._courseMaterialMapper = new Mapper(_configCourseMaterial);
    }

    public async Task<MaterialListViewModel> AddMaterial(MaterialViewModel material)
    {
        var topic = await this._courseTopicRepository.GetTopicByTopicId(
            Guid.Parse(material.TopicId)
        );
        var materialType = this._materialTypeRepository.GetMaterialTypeByMaterialTypeId(
            Guid.Parse(material.MaterialTypeId)
        );
        var isMaterialExists = await this._materialRepository.AnyMaterialByMaterialNameAndTopic(
            material.Name,
            topic
        );
        if (!isMaterialExists)
        {
            // Generate a unique file name
            var uniqueFileName = $"{Guid.NewGuid()}_{material.Material.FileName}";

            // Save the image to a designated folder (e.g., wwwroot/images)
            var uploadsFolder = Path.Combine(this._environment.WebRootPath, "CourseMaterial"); // Use WebRootPath
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                material.Material.CopyTo(stream); // Use await
            }
            var materialCreation = new Material()
            {
                MaterialId = Guid.NewGuid(),
                Name = material.Name,
                MaterialType = materialType,

                CreatedBy = material.CreatedBy,
                CreatedAt = DateTime.Now,
                FilePath = uniqueFileName,

                IsActive = true,
                IsAvailable = true,
                Duration = material.Duration,
                Topic = topic,
                ModifiedAt = null,
                ModifiedBy = null
            };
            await this._materialRepository.AddMaterial(materialCreation);
            return this._courseMaterialMapper.Map<Material, MaterialListViewModel>(
                materialCreation
            );
        }
        else
        {
            return null;
        }
    }

    public async Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndType(
        string topicId,
        string materialTypeId
    )
    {
        var topic = await this._courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
        var materialType = this._materialTypeRepository.GetMaterialTypeByMaterialTypeId(
            Guid.Parse(materialTypeId)
        );

        var material = this._materialRepository.GetAllMaterialDetailsByTopicAndType(
            topic,
            materialType
        );

        List<MaterialListViewModel> materialLists = [];

        foreach (var item in material)
        {
            var materialList = new MaterialListViewModel()
            {
                MaterialId = item.MaterialId,
                TopicName = item.Topic.Name,
                MaterialType = item.MaterialType.Type,
                Name = item.Name,
                // FilePath = item.FilePath,


                FilePath = string.Format(
                    "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                    this._contextAccessor.HttpContext.Request.Scheme,
                    this._contextAccessor.HttpContext.Request.Host,
                    this._contextAccessor.HttpContext.Request.PathBase,
                    item.FilePath
                ),

                Duration = item.Duration,
                IsActive = item.IsActive,
                IsAvailable = item.IsAvailable,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                ModifiedAt = item.ModifiedAt,
                //ModifiedAt = item.ModifiedAt.ToString(),
                ModifiedBy = item.ModifiedBy
            };
            materialLists.Add(materialList);
        }
        return materialLists;
    }

    public async Task<MaterialListViewModel> GetMaterialByMaterialNameAndTopic(
        string materialName,
        string topicId
    )
    {
        var topic = await this._courseTopicRepository.GetTopicByTopicId(Guid.Parse(topicId));
        var material = await this._materialRepository.GetMaterialByMaterialNameAndTopic(
            materialName,
            topic
        );
        var materialView = new MaterialListViewModel()
        {
            MaterialId = material.MaterialId,
            TopicName = material.Topic.Name,
            MaterialType = material.MaterialType.Type,
            Name = material.Name,
            FilePath = string.Format(
                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                material.FilePath
            ),
            Duration = material.Duration,
            IsActive = material.IsActive,
            IsAvailable = material.IsAvailable,
            CreatedAt = material.CreatedAt,
            ModifiedAt = material.ModifiedAt,
            ModifiedBy = material.ModifiedBy,
            CreatedBy = material.CreatedBy
        };
        return materialView;
    }

    public async Task<bool> SoftDeleteMaterial(string materialId)
    {
        var material = await this._materialRepository.GetMaterialByMaterialId(
            Guid.Parse(materialId)
        );
        material.IsActive = false;
        var isMaterialDeleted = await this._materialRepository.UpdateMaterial(material) > 0;
        if (isMaterialDeleted)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateMaterial(MaterialUpdateViewModel material)
    {
        var existMaterial = await this._materialRepository.GetMaterialByMaterialId(
            Guid.Parse(material.MaterialId)
        );
        var materialListByTopic = await this._materialRepository.GetMaterialsByTopic(
            existMaterial.TopicId
        );
        materialListByTopic.Remove(existMaterial);
        var isMaterialNameAlradyExist = materialListByTopic.Any(materials =>
            materials.Name == material.Name
        );
        if (!isMaterialNameAlradyExist)
        {
            var uniqueFileName = $"{Guid.NewGuid()} _{material.Material.FileName}";
            var uploadsFolder = Path.Combine(this._environment.WebRootPath, "CourseMaterial");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await material.Material.CopyToAsync(stream);
            }

            existMaterial.Name = material.Name;
            existMaterial.FilePath = uniqueFileName;
            existMaterial.ModifiedBy = material.ModifiedBy;
            existMaterial.ModifiedAt = DateTime.Now;
            var isMaterialUpdated =
                await this._materialRepository.UpdateMaterial(existMaterial) > 0;
            return isMaterialUpdated;
        }
        else
        {
            return false;
        }

        //    MaterialType materialType = _materialTypeRepository.GetMaterialTypeByMaterialTypeId(Guid.Parse(material.MaterialId));
        //    existingMaterial.MaterialType = materialType;


        //existingMaterial.ModifiedAt = DateTime.Now;
        //existingMaterial.Duration = material.Duration;
        //existingMaterial.IsActive = material.IsActive;
        //existingMaterial.IsAvailable = material.IsAvailable;
    }

    public async Task<MaterialListViewModel> GetMaterialDetailsByMaterialId(string materialId)
    {
        var material = await this._materialRepository.GetMaterialByMaterialId(
            Guid.Parse(materialId)
        );
        var materialView = new MaterialListViewModel()
        {
            MaterialId = material.MaterialId,
            TopicName = material.Topic.Name,
            MaterialType = material.MaterialType.Type,
            Name = material.Name,
            FilePath = string.Format(
                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                material.FilePath
            ),
            Duration = material.Duration,
            IsActive = material.IsActive,
            IsAvailable = material.IsAvailable,
            CreatedAt = material.CreatedAt,
            ModifiedAt = material.ModifiedAt,
            ModifiedBy = material.ModifiedBy,
            CreatedBy = material.CreatedBy
        };
        return materialView;
    }

    public async Task<MaterialListViewModel> GetMaterialDetailsByMaterialIdWithoutPDFConversionForUpdate(
        string materialId
    )
    {
        var material = await this._materialRepository.GetMaterialByMaterialId(
            Guid.Parse(materialId)
        );
        var materialView = new MaterialListViewModel()
        {
            MaterialId = material.MaterialId,
            TopicName = material.Topic.Name,
            MaterialType = material.MaterialType.Type,
            Name = material.Name,
            FilePath = string.Format(
                "{0}://{1}{2}/wwwroot/CourseMaterial/{3}",
                this._contextAccessor.HttpContext.Request.Scheme,
                this._contextAccessor.HttpContext.Request.Host,
                this._contextAccessor.HttpContext.Request.PathBase,
                material.FilePath,
                this._environment,
                this._contextAccessor
            ),
            Duration = material.Duration,
            IsActive = material.IsActive,
            IsAvailable = material.IsAvailable,
            CreatedAt = material.CreatedAt,
            ModifiedAt = material.ModifiedAt,
            ModifiedBy = material.ModifiedBy,
            CreatedBy = material.CreatedBy
        };
        return materialView;
    }
}
