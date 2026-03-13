namespace LXP.Core.Services;

using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;

public class MaterialTypeServices : IMaterialTypeServices // inheriting with Imaterial services
{
    private readonly IMaterialTypeRepository _materialTypeRepository;
    private readonly Mapper _materialTypeMapper;

    public MaterialTypeServices(IMaterialTypeRepository materialTypeRepository)
    {
        var _configMaterialType = new MapperConfiguration(cfg =>
            cfg.CreateMap<MaterialType, MaterialTypeViewModel>().ReverseMap()
        );
        this._materialTypeMapper = new Mapper(_configMaterialType);
        this._materialTypeRepository = materialTypeRepository;
    }

    public List<MaterialTypeViewModel> GetAllMaterialType() =>
        this._materialTypeMapper.Map<List<MaterialType>, List<MaterialTypeViewModel>>(
            this._materialTypeRepository.GetAllMaterialTypes()
        );
}
