using Turbo_Auth.Models.Suppliers;

namespace Turbo_Auth.Repositories.ApiAssets;

public interface IModelRepository
{
    Task<List<Model>?> GetModelsAsync();
    Task<List<Model>?> GetModelsOfKeyAsync(int keyId);
    Task DeleteModelByIdAsync(int id);
    Task AddModelAsync(string name);
    Task UpdateModelAsync(Model model);
    Task<Model?> GetModelByName(string name);
    Task SetEnableStatus(int id,bool enable);
}