using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo_Auth.Models.Suppliers;
using Turbo_Auth.Repositories.ApiAssets;

namespace Turbo_Auth.Controllers.ApiAssets;
[ApiController]
[Authorize(Policy = "admin")]
[Route("api/[controller]")]
public class ModelController: Controller
{
    private IModelRepository _modelRepository;

    public ModelController(IModelRepository modelRepository)
    {
        _modelRepository = modelRepository;
    }

    [HttpGet]
    public async Task<List<Model>?> GetModelsWithKey([FromQuery] int? keyId)
    {
        if (keyId == null)
        {
            return await _modelRepository.GetModelsAsync();
        }

        return await _modelRepository.GetModelsOfKeyAsync(keyId.Value);
    }

    [HttpDelete("{modelId}")]
    public async Task<IActionResult> DeleteModel(int modelId)
    {
        try
        {
            await _modelRepository.DeleteModelByIdAsync(modelId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddModel(Model model)
    {
        try
        {
            await _modelRepository.AddModelAsync(model.Name!);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }

    [HttpPut("{modelId}")]
    public async Task<IActionResult> UpdateModel(Model model,int modelId)
    {
        if (model.ModelId != modelId)
        {
            return BadRequest("不匹配的参数");
        }

        try
        {
            await _modelRepository.UpdateModelAsync(model);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest("执行错误");
        }
    }
}