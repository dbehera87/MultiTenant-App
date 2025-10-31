using License.Persistence.Repositories;

namespace License.Application.Commands
{
    public class IssueLicense
    {
        private readonly ILicenseRepository repository;

        public IssueLicense(ILicenseRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Process(IssueLicenseCommand request, string tenantId)
        {
            var license = new Domain.Entities.Licenses
            {
                LicenseNumber = request.LicenseNumber,
                LicenseType = request.LicenseType,
                ExpirationDate = request.ExpirationDate,
                Status = "Active"
            };
            int licenseId = await repository.AddAsync(license, tenantId);
            return licenseId;
        }
    }
}
