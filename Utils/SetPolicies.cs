using Microsoft.AspNetCore.Cors.Infrastructure;

namespace InventorySystem_webapi.Utils
{
    public static class SetPolicies
    {
        public static void SetAppPolicies(this CorsPolicyBuilder policy)
        {
            policy
                .AllowAnyOrigin() // in production use .WithOrigins("https://domain.com", "https://otherclient.com")
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    }
}
