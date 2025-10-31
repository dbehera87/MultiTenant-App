using License.Domain.Entities;
using License.Persistence.Config;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace License.Persistence.Repositories
{
    public class LicenseRepository : BaseRepository, ILicenseRepository
    {
        public LicenseRepository(IOptions<SqlConnectionConfig> config): base(config)
        {

        }

        public async Task<int> AddAsync(Licenses license, string tenantId)
        {
            const string sql = @"INSERT INTO Licenses (LicenseNumber, LicenseType, Status, ExpirationDate, TenantId) 
                                 VALUES (@LicenseNumber, @LicenseType, @Status, @ExpirationDate, @TenantId);
                                 SELECT SCOPE_IDENTITY();";

            var parameters = new List<SqlParameter>
            {
                //new("@LicenseId", license.LicenseId),
                new("@LicenseNumber", license.LicenseNumber),
                new("@LicenseType", license.LicenseType),
                new("@Status", license.Status),
                new("@ExpirationDate", license.ExpirationDate),
                new("@TenantId", tenantId)
            };

            var result = await ExecuteScalarAsync(sql, parameters);
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<Licenses>> GetAllAsync(string tenantId)
        {
            const string sql = "SELECT LicenseId, LicenseNumber, LicenseType, Status, ExpirationDate, TenantId FROM Licenses";
            return await ExecuteReaderAsync(sql, reader => new Licenses
            {
                LicenseId = reader.GetInt32(0),
                LicenseNumber = reader.GetString(1),
                LicenseType = reader.GetString(2),
                Status = reader.GetString(3),
                ExpirationDate = reader.GetDateTime(4),
                TenantId = reader.GetString(5)
            });
        }

        public async Task<Licenses> GetByIdAsync(int id, string tenantId)
        {
            const string sql = "SELECT LicenseId, LicenseNumber, LicenseType, Status, ExpirationDate, TenantId FROM Licenses WHERE LicenseId = @LicenseId AND TenantId = @TenantId";
            var parameters = new List<SqlParameter> { new("@LicenseId", id) };

            var licenses = await ExecuteReaderAsync(sql, reader => new Licenses
            {
                LicenseId = reader.GetInt32(0),
                LicenseNumber = reader.GetString(1),
                LicenseType = reader.GetString(2),
                Status = reader.GetString(3),
                ExpirationDate = reader.GetDateTime(4),
                TenantId = reader.GetString(5)
            }, parameters);

            return licenses.FirstOrDefault() ?? new();
        }

        public async Task<int> UpdateAsync(Licenses license, string tenantId)
        {
            const string sql = @"UPDATE Licenses 
                                 SET Status = @Status, ExpirationDate = @ExpirationDate 
                                 WHERE LicenseId = @LicenseId AND TenantId = @TenantId";

            var parameters = new List<SqlParameter>
            {
                new("@LicenseId", license.LicenseId),
                //new("@LicenseNumber", license.LicenseNumber),
                //new("@LicenseType", license.LicenseType),
                new("@Status", license.Status),
                new("@ExpirationDate", license.ExpirationDate),
                //new("@TenantId", license.TenantId)
            };

            return await ExecuteNonQueryAsync(sql, parameters);
        }

        public async Task<int> DeleteAsync(int id, string tenantId)
        {
            const string sql = "DELETE FROM Licenses WHERE LicenseId = @LicenseId AND TenantId = @TenantId";
            var parameters = new List<SqlParameter> { new("@Id", id), new("@TenantId", tenantId) };

            return await ExecuteNonQueryAsync(sql, parameters);
        }
    }
}
