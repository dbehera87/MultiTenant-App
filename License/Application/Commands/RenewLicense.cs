using License.Persistence.Repositories;

namespace License.Application.Commands
{
    public class RenewLicense
    {
        private readonly ILicenseRepository repository;

        public RenewLicense(ILicenseRepository repository)
        {
            this.repository = repository;
        }
        public async Task<int> Process(RenewLicenseCommand request, string tenantId)
        {
            var license = new Domain.Entities.Licenses
            {
                LicenseNumber = request.LicenseNumber,
                ExpirationDate = request.ExpirationDate,
                Status = "Active"
            };
            int licenseId = await repository.UpdateAsync(license, tenantId);
            return licenseId;
        }
    }
}
