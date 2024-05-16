using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Requester;
using Newtonsoft.Json;
using Turbo_Auth.Handlers.Model2Key;
using Turbo_Auth.Models.Ai;
using Turbo_Auth.Models.Ai.Chat;
using Part = DotnetGeminiSDK.Model.Request.Part;

namespace Turbo_Auth.Handlers.Chat;
[Obsolete]
public class GoogleChatHandler: IChatHandler
{
    public async Task Chat(NoModelChatBody chatBody, ModelKey modelKey,HttpResponse response)
    {
        var messages = TransferObject(chatBody.Messages);
        var geminiClient = new GeminiClient(
            new GoogleGeminiConfig()
            {
                ApiKey = modelKey.SupplierKey!.ApiKey!
            },
            new ApiRequester()
            );
        await geminiClient.StreamTextPrompt(messages, async (chunck) =>
        {
            chunck = FillBlock(chunck);
            var geminiParts = JsonConvert.DeserializeObject<GeminiPart[]>(chunck);
            if (geminiParts == null) return;
            foreach (var block in geminiParts)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (block != null)
                {
                    await response.WriteAsync(block!.Candidates![0]!.Content.Parts[0].Text);
                }
            }
        });
    }

    private List<DotnetGeminiSDK.Model.Request.Content> TransferObject(Message[]? chatBodyMessages)
    {
        var parts = new List<DotnetGeminiSDK.Model.Request.Content>();
        foreach (var message in chatBodyMessages!)
        {
            switch (message.Role!.ToLower())
            {
                case OpenAiRole.SystemRole:
                    parts.Add(new ()
                    {
                        Role = GoogleRoles.User,
                        Parts = new List<Part>()
                        {
                            new ()
                            {
                                Text = message!.Content!,
                            }
                        }
                    });
                    break;
                case OpenAiRole.UserRole:
                    parts.Add(new ()
                    {
                        Role = GoogleRoles.User,
                        Parts = new List<Part>()
                        {
                            new ()
                            {
                                Text = message!.Content!
                            }
                        }
                    });
                    break;
                case OpenAiRole.Assistant:
                    parts.Add(new ()
                    {
                        Role = GoogleRoles.Model,
                        Parts = new List<Part>()
                        {
                            new ()
                            {
                                Text = message!.Content!
                            }
                        } 
                    });
                    break;
                default:
                    parts.Add(new ()
                    {
                        Role = GoogleRoles.User,
                        Parts = new List<Part>()
                        {
                            new ()
                            {
                                Text = message!.Content!,
                            }
                        }
                    });
                    break;
            }
        }

        return parts;
    }

    private string FillBlock(string chunck)
    {
        var light = chunck.Trim();
        if (!light.StartsWith('['))
        {
            light = '[' + light;
        }

        if (!light.EndsWith(']'))
        {
            light += ']';
        }

        return light;
    }
}

public class GoogleRoles
{
    public const string User = "user";
    public const string Model = "model";
}