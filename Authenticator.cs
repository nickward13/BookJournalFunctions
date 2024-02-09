using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Hectagon.BookJournal
{
    public static class Authenticator
    {
        public static string GetUserId(HttpRequest req)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            ClaimsPrincipal principal = req.HttpContext.User;

            if (environment == "Development" && principal.Identity.IsAuthenticated == false)
            {
                principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "testuser"),
                    new Claim(ClaimTypes.NameIdentifier, "testuser")
                }, "TestAuthentication"));
            }

            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }

            return principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}