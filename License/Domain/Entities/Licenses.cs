using SharedTenant;

namespace License.Domain.Entities
{
    public class Licenses : AuditEntity, ITenantEntity
    {
        public int LicenseId { get; set; }
        public string LicenseNumber { get; set; } = default!;
        public string LicenseType { get; set; } = default!;
        public string Status { get; set; } = "Pending";
        public string Applicant { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
