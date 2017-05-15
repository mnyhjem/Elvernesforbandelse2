using System;
using System.Security.Claims;

namespace Elvencurse2.Server.Utilities
{
    public static class Extensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? claim.Value : null;
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.Name);
            return claim != null ? claim.Value : null;
        }
    }
}
