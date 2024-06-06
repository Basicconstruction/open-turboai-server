﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Turbo_Auth.Models.Ai.Media.STT;

public class OpenAiTranscriptionRequest
{
    [Required]
    [JsonProperty("file")]
    public string? File
    {
        get;
        set;
    }
    [Required]
    [JsonProperty("model")]
    public string? Model
    {
        get;
        set;
    }

    [JsonProperty("language")]
    public string? Language
    {
        get;
        set;
    }
//An optional text to guide the model's style or continue a previous audio segment. The prompt should match the audio language
    [JsonProperty("prompt")]
    public string? Prompt
    {
        get;
        set;
    }

    [JsonProperty("temperature")]
    public float Temperature
    {
        get;
        set;
    }
    [JsonProperty("response_format")]
    public string? ResponseFormat
    {
        get;
        set;
    }
    [JsonProperty("suffix")]
    [Required]
    public string? Suffix
    {
        get;
        set;
    }

    public override string ToString()
    {
        return $"OpenAiTranslationRequest{{model='{Model}', prompt='{Prompt}', temperature={Temperature}, responseFormat='{ResponseFormat}', suffix='{Suffix}', language='{Language}'}}";

    }
}