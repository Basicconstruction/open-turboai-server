using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turbo_Auth.Handlers.Chat;
using Turbo_Auth.Handlers.Differentiator;
using Turbo_Auth.Handlers.Group;
using Turbo_Auth.Handlers.Model;
using Turbo_Auth.Handlers.Model2Key;
using Turbo_Auth.Models.Ai.Chat;

namespace Turbo_Auth.Controllers.Transfer;
[Authorize(Policy = "vip")]
[ApiController]
[Route("api/ai")]
public class ChatController: Controller
{
    private IChatHandlerObtain _chatHandlerObtain;
    private QuickModel _quickModel;
    private PlayMixModelBacker _backer;
    public ChatController(IChatHandlerObtain chatHandlerObtain, 
        QuickModel quickModel,PlayMixModelBacker backer
    )
    {
        _chatHandlerObtain = chatHandlerObtain;
        _quickModel = quickModel;
        _backer = backer;
    }
    
    
    [HttpPost("chat")]
    public async Task Chat(NoModelChatBody chatBody)
    {
        try
        {
            var modelKey = _quickModel.GetModelAndKey(chatBody.Model!);
            modelKey!.Model = _backer.Backer(modelKey.Model!);
            var handler = _chatHandlerObtain.GetHandler
                ((HandlerType)modelKey!.SupplierKey!.RequestIdentifier);
            await handler.Chat(chatBody,modelKey,Response);
        }
        catch (Exception e)
        {
            await Response.WriteAsync(e.Message);
            await Response.CompleteAsync();
        }
    }

    [HttpGet("models")]
    // [Authorize("user")]
    public List<ChatDisplayModel> GetChatModels()
    {
        var group = new ModelGroup(true);
        return group.Group[0];
    }
}