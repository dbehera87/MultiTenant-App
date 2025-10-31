namespace LicenseUI.Models
{
    public class LicenseViewModel
    {
        public int LicenseId { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public string Status { get; set; }
        public string Applicant { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
