using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using Turbo_Auth.Handlers.Model2Key;
using Turbo_Auth.Models.Ai;
using Turbo_Auth.Models.Ai.Chat;

namespace Turbo_Auth.Handlers.Chat;

public class OpenAiChatHandler : IChatHandler
{
    
    public async Task Chat(NoModelChatBody chatBody, ModelKey modelKey,HttpResponse response)
    {
        var openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = modelKey.SupplierKey!.ApiKey!,
            BaseDomain = modelKey.SupplierKey.BaseUrl!
        });
        var completionResult = openAiService.ChatCompletion.CreateCompletionAsStream(new ChatCompletionCreateRequest
        {
            Messages = TransferObject(chatBody.Messages!,chatBody.Vision),
            Model = modelKey.Model,
            MaxTokens = chatBody.MaxTokens,
            TopP = (float?)chatBody.TopP,
            PresencePenalty = (float?)chatBody.PresencePenalty,
        });

        await foreach (var completion in completionResult)
        {
            if (completion.Successful)
            {
                if (completion.Choices.FirstOrDefault() != null)
                {
                    if (completion.Choices.FirstOrDefault()?.Message.Content != null)
                    {
                        if (completion.Choices.FirstOrDefault()?.Message.Content!.Length > 0)
                        {
                            await response.WriteAsync(completion.Choices.FirstOrDefault()?.Message.Content!);
                        }

                    }
                }
            }
            else
            {
                if (completion.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                await response.WriteAsync($"{completion.Error.Code}: {completion.Error.Message}");
            }
        }

        await response.CompleteAsync();
    }

    private static List<ChatMessage> TransferObject(IEnumerable<Message> messages,bool vision=false)
    {
        var ms = new List<ChatMessage>();
        foreach (var message in messages)
        {
            switch (message.Role!.ToLower()!)
            {
                case OpenAiRole.SystemRole:
                    ms.Add(ChatMessage.FromSystem(message.Content!));
                    break;
                case OpenAiRole.UserRole:
                    if (vision)
                    {
                        var mcl = new List<MessageContent>();
                        foreach (var vc in (message.Content as VisionContent[])!)
                        {
                            mcl.Add(new MessageContent()
                            {
                                Type = vc.Type!,
                                Text = vc.Text,
                                ImageUrl = new VisionImageUrl()
                                {
                                    Url = vc.VisionImage!.Url!,
                                    Detail = vc.VisionImage!.Detail
                                }
                            });
                        }
                        ms.Add(ChatMessage.FromUser(mcl));
                    }
                    else
                    {
                        ms.Add(ChatMessage.FromUser(message.Content!));
                    }
                    
                    break;
                case OpenAiRole.Assistant:
                    ms.Add(ChatMessage.FromAssistant(message.Content!));
                    break;
                default:
                    ms.Add(ChatMessage.FromUser(message.Content!));
                    break;
            }
        }
        return ms;
    }
}

public class OpenAiRole
{
    public const string SystemRole = "system";
    public const string Assistant = "assistant";
    public const string UserRole = "user";
}