using License.Application.Commands;
using License.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedTenant;

namespace License.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly IssueLicense issueLicense;
        private readonly RenewLicense renewLicense;
        private readonly GetLicenses getLicenses;
        private readonly GetLicenseById getLicenseById;

        public LicenseController(IssueLicense issueLicense, RenewLicense renewLicense, GetLicenses getLicenses, GetLicenseById getLicenseById)
        {
            this.issueLicense = issueLicense;
            this.renewLicense = renewLicense;
            this.getLicenses = getLicenses;
            this.getLicenseById = getLicenseById;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IssueLicenseCommand request)
        {
            var id = await issueLicense.Process(request, TenantStore.TenantId);
            return Ok(new { LicenseId = id });
        }

        [HttpPatch]
        public async Task<IActionResult> RenewLicense(int licenseId, RenewLicenseCommand request)
        {
            var updatedId = await renewLicense.Process(request, TenantStore.TenantId);
            return Ok(new { LicenseId = updatedId });
        }

        [HttpGet]
        public async Task<IActionResult> GetLicenses()
        {
            var licenses = await getLicenses.Process(TenantStore.TenantId);
            return Ok(new { Licenses = licenses });
        }

        [HttpGet("{licenseId}")]
        public async Task<IActionResult> GetLicense(int licenseId)
        {
            var license = await getLicenseById.Process(licenseId, TenantStore.TenantId);
            return Ok(new { License = license });
        }
    }
}
