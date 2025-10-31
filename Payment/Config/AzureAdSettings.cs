namespace Payment.Config
{
    public class AzureAdSettings
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DownstreamScopes { get; set; }
    }
}