using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Hectagon.BookJournal
{
    public static class Authenticator
    {
        public static string? GetUserId(HttpRequest req)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string? userId = req.Headers["x-ms-client-principal-id"];

            if (string.IsNullOrEmpty(userId))
            {
                if (environment == "Development")
                {
                    userId = "testuser";
                }
                else
                {
                    return null;
                }
            }

            return userId;
        }
    }
}