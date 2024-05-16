using System.Security.Claims;

namespace Turbo_Auth.Controllers.Sync;

public interface IIdGetter
{
    int GetId(ClaimsPrincipal User);
}