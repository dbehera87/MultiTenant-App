using License.Persistence.Repositories;

namespace License.Application.Queries
{
    public class GetLicenseById
    {
        private readonly ILicenseRepository repository;

        public GetLicenseById(ILicenseRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Domain.Entities.Licenses> Process(int licenseId, string tenantId)
        {
            var license = await repository.GetByIdAsync(licenseId, tenantId);
            return license;
        }
    }
}
