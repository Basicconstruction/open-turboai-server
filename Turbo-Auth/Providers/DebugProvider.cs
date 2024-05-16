namespace Turbo_Auth.Providers;
[Obsolete]
public class DebugProvider
{
    public DebugKey Provide()
    {
        return new DebugKey()
        {
            Key = "",
            BaseUrl = "https://api.chatanywhere.com.cn/v1"
        };
    }
}