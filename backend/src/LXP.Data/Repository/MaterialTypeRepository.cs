namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;

public class MaterialTypeRepository(LXPDbContext lXPDbContext) : IMaterialTypeRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext; // through this connection with table in db

    public MaterialType GetMaterialTypeByMaterialTypeId(Guid materialTypeId) =>
        this._lXPDbContext.MaterialTypes.Find(materialTypeId);

    public List<MaterialType> GetAllMaterialTypes() => this._lXPDbContext.MaterialTypes.ToList();
}
