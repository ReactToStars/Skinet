using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            // return user?.Claims?.FirstOrDefault(u => u.Type == ClaimTypes.Email).Value;
            return user?.FindFirstValue(ClaimTypes.Email);
        }
    }
}