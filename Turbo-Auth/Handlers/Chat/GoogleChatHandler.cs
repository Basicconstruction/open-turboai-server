using System.Diagnostics;
using DotnetGeminiSDK.Client;
using DotnetGeminiSDK.Config;
using DotnetGeminiSDK.Model;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Requester;
using Newtonsoft.Json;
using Turbo_Auth.Handlers.Model2Key;
using Turbo_Auth.Models.Ai.Chat;
using Part = DotnetGeminiSDK.Model.Request.Part;

namespace Turbo_Auth.Handlers.Chat;

[Obsolete]
public class GoogleChatHandler : IChatHandler
{
    public async Task Chat(NoModelChatBody chatBody, ModelKey modelKey, HttpResponse response)
    {
        var messages = TransferObject(chatBody.Messages, chatBody.Vision);
        var geminiClient = new GeminiClient(
            new GoogleGeminiConfig()
            {
                ApiKey = modelKey.SupplierKey!.ApiKey!
            },
            new ApiRequester()
        );
        if (chatBody.Vision)
        {
            var message = chatBody.Messages.Last();
            var vc = JsonConvert.DeserializeObject<VisionMessage>(JsonConvert.SerializeObject(message))!.Content;
            var text = "";
            var base64Image = "";
            var type = "";
            foreach (var v in vc)
            {
                if (v.Type == "text")
                {
                    text = v.Text;
                }
                else
                {
                    var inlineData = GetInlineData(v.VisionImage.Url);
                    base64Image = inlineData.Data;
                    type = inlineData.MimeType;
                }
            }

            var mimeType = type switch
            {
                "image/jpeg" => ImageMimeType.Jpeg,
                "image/jpg" => ImageMimeType.Jpg,
                "image/png" => ImageMimeType.Png,
                "image/webp" => ImageMimeType.Webp,
                "image/heic" => ImageMimeType.Heic,
                "image/heif" => ImageMimeType.Heif,
                _ => ImageMimeType.Jpeg
            };

            var res = await geminiClient.ImagePrompt(text, base64Image,mimeType);
            // foreach (var ms in res.Candidates)
            // {
            //     foreach (var part in ms.Content.Parts)
            //     {
            //         Console.WriteLine(part);
            //     }
            // }
        }
        else
        {
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
    }

    private static List<Content> TransferObject(Message[]? chatBodyMessages, bool vision = false)
    {
        var parts = new List<Content>();

        foreach (var message in chatBodyMessages!)
        {
            switch (message.Role!.ToLower())
            {
                case OpenAiRole.SystemRole:
                    parts.Add(new()
                    {
                        Role = GoogleRoles.User,
                        Parts = new List<Part>()
                        {
                            new()
                            {
                                Text = message!.Content! as string,
                            }
                        }
                    });
                    break;
                case OpenAiRole.UserRole:
                    parts.Add(new()
                    {
                        Role = GoogleRoles.User,
                        Parts = new List<Part>()
                        {
                            new()
                            {
                                Text = message!.Content! as string
                            }
                        }
                    });

                    break;
                case OpenAiRole.Assistant:
                    parts.Add(new()
                    {
                        Role = GoogleRoles.Model,
                        Parts = new List<Part>()
                        {
                            new()
                            {
                                Text = message!.Content! as string
                            }
                        }
                    });
                    break;
                default:
                    parts.Add(new()
                    {
                        Role = GoogleRoles.User,
                        Parts = new List<Part>()
                        {
                            new()
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

    private static InlineData? GetInlineData(string url)
    {
        var parts = url.Split(',');
        var mimeType = parts[0].Split(':')[1].Split(';')[0];
        var data = parts[1];
        return new InlineData()
        {
            Data = data,
            MimeType = mimeType
        };
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