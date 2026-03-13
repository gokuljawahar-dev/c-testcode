namespace LXP.Data.Repository;

using LXP.Common.Entities;
using LXP.Data.IRepository;
using Microsoft.EntityFrameworkCore;

public class MaterialRepository(LXPDbContext lXPDbContext) : IMaterialRepository
{
    private readonly LXPDbContext _lXPDbContext = lXPDbContext;

    public async Task AddMaterial(Material material)
    {
        await this._lXPDbContext.Materials.AddAsync(material);
        await this._lXPDbContext.SaveChangesAsync();
    }

    public async Task<bool> AnyMaterialByMaterialNameAndTopic(string materialName, Topic topic) =>
        await this._lXPDbContext.Materials.AnyAsync(material =>
            material.Name == materialName && material.Topic == topic
        );

    public async Task<Material> GetMaterialByMaterialNameAndTopic(
        string materialName,
        Topic topic
    ) =>
        await this._lXPDbContext.Materials.FirstOrDefaultAsync(material =>
            material.Name == materialName && material.Topic == topic
        );

    public List<Material> GetAllMaterialDetailsByTopicAndType(
        Topic topic,
        MaterialType materialType
    ) =>
        this
            ._lXPDbContext.Materials.Include(material => material.MaterialType)
            .Include(material => material.Topic)
            .Where(material =>
                material.IsActive
                && material.Topic == topic
                && material.MaterialType == materialType
            )
            .Include(material => material.Topic)
            .Include(material => material.MaterialType)
            .ToList();

    public async Task<Material> GetMaterialById(Guid materialId) =>
        await this._lXPDbContext.Materials.FirstOrDefaultAsync(material =>
            material.MaterialId == materialId
        );

    public async Task<List<Material>> GetMaterialsByTopic(Guid topic) =>
        await this
            ._lXPDbContext.Materials.Where(material => material.TopicId == topic)
            .ToListAsync();

    public async Task<Material> GetMaterialByMaterialId(Guid materialId) =>
        await this
            ._lXPDbContext.Materials.Include(material => material.MaterialType)
            .Include(material => material.Topic)
            .FirstAsync(material => material.MaterialId == materialId);

    //end
    public async Task<int> UpdateMaterial(Material material)
    {
        this._lXPDbContext.Materials.Update(material);
        return await this._lXPDbContext.SaveChangesAsync();
    }

    //         material.MaterialId == materialId);
}
