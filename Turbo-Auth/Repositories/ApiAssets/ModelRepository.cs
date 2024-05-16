using Microsoft.EntityFrameworkCore;
using Turbo_Auth.Context;
using Turbo_Auth.Models.Suppliers;

namespace Turbo_Auth.Repositories.ApiAssets;

public class ModelRepository: IModelRepository
{
    private KeyContext _keyContext;

    public ModelRepository(KeyContext keyContext)
    {
        _keyContext = keyContext;
    }

    public async Task<List<Model>?> GetModelsAsync()
    {
        return await _keyContext.Models!.ToListAsync();
    }

    public async Task<List<Model>?> GetModelsOfKeyAsync(int keyId)
    {
        return await _keyContext.ModelFees!.Where(f => f.SupplierKeyId == keyId)
            .Include(f => f.Model)
            .Select(f => f.Model!)
            .ToListAsync();
    }

    public async Task DeleteModelByIdAsync(int id)
    {
        var modelFees = await _keyContext.ModelFees!
            .Where(f => f.ModelId == id)
            .ToListAsync();
        if (modelFees.Count != 0)
        {
            _keyContext.ModelFees!.RemoveRange(modelFees);
            await _keyContext.SaveChangesAsync();
        }
        var model = await _keyContext.Models!.Where(m => m.ModelId == id).FirstOrDefaultAsync();
        if (model != null)
        {
            _keyContext.Models!.Remove(model);
        }

        await _keyContext.SaveChangesAsync();
    }

    public async Task AddModelAsync(string name)
    {
        var exists = await _keyContext.Models!.AnyAsync(m => m.Name == name);
        if (!exists)
        {
            _keyContext.Models!.Add(new Model()
            {
                Name = name
            });
            await _keyContext.SaveChangesAsync();
        }
    }

    public async Task UpdateModelAsync(Model model)
    {
        var innerModel = await _keyContext.Models!
            .FirstOrDefaultAsync(m => m.ModelId == model.ModelId);
        if (innerModel != null)
        {
            innerModel.Name = model.Name;
            await _keyContext.SaveChangesAsync();
        }
        
    }

    public async Task<Model?> GetModelByName(string name)
    {
        return await _keyContext.Models!.FirstOrDefaultAsync(m => m.Name == name);
    }
}