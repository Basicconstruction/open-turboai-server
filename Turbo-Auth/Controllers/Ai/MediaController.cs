using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using Turbo_Auth.Handlers.Model2Key;

namespace Turbo_Auth.Controllers.Ai;

[ApiController]
// [Authorize("vip")]
[Route("api/[controller]")]
public class MediaController : Controller
{
    private QuickModel _quickModel;

    public MediaController(
        QuickModel quickModel
    )
    {
        _quickModel = quickModel;
    }

    [HttpPost("tts")]
    public async Task<IActionResult> TTS(AudioCreateSpeechRequest speechRequest)
    {
        var modelKey = _quickModel.GetModelAndKey(speechRequest.Model);
        var openaiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = modelKey!.SupplierKey!.ApiKey!,
            BaseDomain = modelKey.SupplierKey.BaseUrl!
        });
        var ttsResult = 
            await openaiService.Audio.CreateSpeech<byte[]>(speechRequest);
        return Ok(new 
        {
            base64 = Convert.ToBase64String(ttsResult.Data!),
            type = speechRequest.ResponseFormat
        });
    }

    [HttpPost("whisper-translate")]
    public async Task<IActionResult> WhisperTranslate(AudioCreateTranscriptionRequest transcriptionRequest)
    {
        var modelKey = _quickModel.GetModelAndKey(transcriptionRequest.Model);
        var openaiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = modelKey!.SupplierKey!.ApiKey!,
            BaseDomain = modelKey.SupplierKey.BaseUrl!
        });
        var translateResult = await openaiService.Audio.CreateTranslation(transcriptionRequest);
        
        return Ok();
    }
    [HttpPost("whisper-transcription")]
    public async Task<IActionResult> WhisperTranscription(AudioCreateTranscriptionRequest transcriptionRequest)
    {
        var modelKey = _quickModel.GetModelAndKey(transcriptionRequest.Model);
        var openaiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = modelKey!.SupplierKey!.ApiKey!,
            BaseDomain = modelKey.SupplierKey.BaseUrl!
        });
        var transcriptionResult = await openaiService.Audio.CreateTranscription(transcriptionRequest);
        
        return Ok();
    }

    [HttpPost("dall-e-3")]
    public async Task<IActionResult> Dall_E_3(ImageCreateRequest imageCreate)
    {
        var modelKey = _quickModel.GetModelAndKey(imageCreate.Model!);
        var openaiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = modelKey!.SupplierKey!.ApiKey!,
            BaseDomain = modelKey.SupplierKey.BaseUrl!
        });
        var imageResult = await openaiService.Image.CreateImage(imageCreate);
        return Ok();
    }
}