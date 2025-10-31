using Microsoft.AspNetCore.Http;

namespace SharedTenant
{
    public class TenantStore
    {
        public static string TenantId { get; private set; }

        public TenantStore(IHttpContextAccessor httpContextAccessor)
        {
            TenantId = httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault() ?? string.Empty;
        }
    }
}
