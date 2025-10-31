namespace License.Persistence.Repositories
{
    public interface ILicenseRepository
    {
        Task<Domain.Entities.Licenses> GetByIdAsync(int id, string tenantId); // Fully qualify the License type
        Task<IEnumerable<Domain.Entities.Licenses>> GetAllAsync(string tenantId); // Fully qualify the License type
        Task<int> AddAsync(Domain.Entities.Licenses license, string tenantId); // Fully qualify the License type
        Task<int> UpdateAsync(Domain.Entities.Licenses license, string tenantId); // Fully qualify the License type
        Task<int> DeleteAsync(int id, string tenantId); // Fully qualify the License type
    }
}
