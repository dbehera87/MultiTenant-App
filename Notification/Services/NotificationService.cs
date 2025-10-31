using Azure.Messaging.ServiceBus;
using Notification.Models;
using System.Text.Json;

namespace Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<NotificationService> logger;

        public NotificationService(IConfiguration configuration, ILogger<NotificationService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task SendNotificationAsync(NotificationMessage message, CancellationToken cancellationToken = default)
        {
            string connectionString;
            if ((connectionString = configuration.GetConnectionString("ServiceBus")) != null)
            {
                await using var client = new ServiceBusClient(connectionString);
                await using var sender = client.CreateSender("notifications-queue");

                await sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(message)), cancellationToken);
            }
            else
            {
                string errorMessage = "No Valid ConnectionString found for Service Bus Queue Connection";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage); 
            }

            logger.LogInformation("Queued email notification for {Email}", message.RecipientEmail);
        }
    }
}
