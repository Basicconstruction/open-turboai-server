using System.Text;
using Newtonsoft.Json;

namespace Turbo_Auth.Models.Ai.Chat;
public class NoModelChatBody
{
    [JsonProperty("messages")]
    public Message[]? Messages { get; set; }
    
    [JsonProperty("model")]
    public string? Model { get; set; }
    
    [JsonProperty("frequency_penalty")]
    public double? FrequencyPenalty { get; set; }
    [JsonProperty("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonProperty("presence_penalty")]
    public double? PresencePenalty { get; set; }
    
    [JsonProperty("stream")]
    public bool Stream { get; set;}
    
    [JsonProperty("temperature")] 
    public double? Temperature { get; set;}

    [JsonProperty("top_p")] 
    public double? TopP {get;set;}
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var message in Messages!)
        {
            sb.AppendLine(message.ToString());
        }
        return $"ChatBody: Messages=\n{sb}, " +
               $"Model={Model}, FrequencyPenalty={FrequencyPenalty}, MaxTokens={MaxTokens}, PresencePenalty={PresencePenalty}, Stream={Stream}, Temperature={Temperature}, TopP={TopP}";
    }
}

public class Message
{
    [JsonProperty("role")]
    public string? Role { get; set; }

    [JsonProperty("content")]
    public string? Content { get; set; }

    public override string ToString()
    {
        return $"Message: Role={Role}, Content={Content}";
    }
}
