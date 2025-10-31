using Notification.Models;

namespace Notification.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    }
}
