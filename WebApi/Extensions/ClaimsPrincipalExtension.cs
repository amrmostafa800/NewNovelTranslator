using System.Security.Claims;

namespace WebApi.Extensions;

public static class ClaimsPrincipalExtension
{
    public static int GetCurrentUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToInt();
    }
}