namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IMaterialTypeRepository
{
    MaterialType GetMaterialTypeByMaterialTypeId(Guid materialTypeId); // getting the material type using id
    List<MaterialType> GetAllMaterialTypes(); // get all material type available in the table
}
