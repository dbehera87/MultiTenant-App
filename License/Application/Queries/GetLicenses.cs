using AutoMapper;
using License.Domain.Dto;
using License.Persistence.Repositories;

namespace License.Application.Queries
{
    public class GetLicenses
    {
        private readonly ILicenseRepository licenseRepository;
        private readonly IMapper mapper;

        public GetLicenses(ILicenseRepository licenseRepository, IMapper mapper)
        {
            this.licenseRepository = licenseRepository;
            this.mapper = mapper;
        }
        public async Task<List<LicenseDto>> Process(string tenantId)
        {
            var model = await licenseRepository.GetAllAsync(tenantId);
            var result = mapper.Map<List<LicenseDto>>(model);
            return result;
        }
    }
}
