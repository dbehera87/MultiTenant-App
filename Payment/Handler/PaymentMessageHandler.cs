using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Payment.Config;
using System.Net.Http.Headers;

namespace Payment.Handler
{
    public class PaymentMessageHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AzureAdSettings configuration;

        public PaymentMessageHandler(IOptions<AzureAdSettings> configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration.Value;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            var incomingAccessToken = authHeader?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(incomingAccessToken))
                throw new UnauthorizedAccessException("Missing bearer token in incoming request.");

            var app = ConfidentialClientApplicationBuilder
                .Create(configuration.ClientId)
                .WithClientSecret(configuration.ClientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{configuration.TenantId}")
                .Build();

            var result = await app
                .AcquireTokenOnBehalfOf(configuration.DownstreamScopes.Split(","), new UserAssertion(incomingAccessToken))
                .ExecuteAsync(cancellationToken);

            // Attach the new token to the outbound request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
