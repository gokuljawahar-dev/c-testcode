namespace LXP.Core.IServices;

using LXP.Common.ViewModels;

public interface IMaterialServices
{
    Task<List<MaterialListViewModel>> GetAllMaterialDetailsByTopicAndType(
        string topicId,
        string materialTypeId
    ); // get
    Task<MaterialListViewModel> AddMaterial(MaterialViewModel material);
    Task<MaterialListViewModel> GetMaterialByMaterialNameAndTopic(
        string materialName,
        string topicId
    );
    Task<bool> SoftDeleteMaterial(string materialId);
    Task<bool> UpdateMaterial(MaterialUpdateViewModel material);

    Task<MaterialListViewModel> GetMaterialDetailsByMaterialId(string materialId);

    Task<MaterialListViewModel> GetMaterialDetailsByMaterialIdWithoutPDFConversionForUpdate(
        string materialId
    );
}
