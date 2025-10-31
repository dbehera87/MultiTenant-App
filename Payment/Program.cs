using Microsoft.Identity.Web;
using Payment.Config;
using Payment.Gateway;
using Payment.Handler;
using Payment.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.Configure<ExternalServiceConfig>(builder.Configuration.GetSection("ExternalServiceConfig"));
builder.Services.Configure<AzureAdSettings>(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddTransient<IPaymentGateway, PaymentGateway>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

builder.Services.AddHttpClient<IPaymentGateway, PaymentGateway>(gateway =>
{
    gateway.BaseAddress = new Uri("https://extservice.com");
    gateway.Timeout = TimeSpan.FromSeconds(300);
})
.AddHttpMessageHandler<PaymentMessageHandler>()
.AddPolicyHandler(Policy.WrapAsync(
    PolicyForRetry(),
    PolicyForHttp()
));

//Retry policy for retrying 3 times with exponential backoff
IAsyncPolicy<HttpResponseMessage> PolicyForRetry()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryAttempt, context) => { }
        );
}

//Circuit Breaker Policy for checking the external service health
IAsyncPolicy<HttpResponseMessage> PolicyForHttp()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (result, breakelay) => { },
            onReset: () => { },
            onHalfOpen: () => { }
        );
}

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
