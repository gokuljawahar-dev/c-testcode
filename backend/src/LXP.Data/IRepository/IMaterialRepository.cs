namespace LXP.Data.IRepository;

using LXP.Common.Entities;

public interface IMaterialRepository
{
    List<Material> GetAllMaterialDetailsByTopicAndType(Topic topic, MaterialType materialType); // get
    Task AddMaterial(Material material); //add
    Task<bool> AnyMaterialByMaterialNameAndTopic(string materialName, Topic topic);
    Task<Material> GetMaterialByMaterialNameAndTopic(string materialName, Topic topic);
    Task<Material> GetMaterialById(Guid materialId);
    Task<List<Material>> GetMaterialsByTopic(Guid topic);
    Task<int> UpdateMaterial(Material material);
    Task<Material> GetMaterialByMaterialId(Guid materialId);

    //  Task<Material> GetmeterialDurationByMaterialId(Guid materialId);
}
