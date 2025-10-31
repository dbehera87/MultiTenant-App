using Microsoft.AspNetCore.Mvc;
using Notification.Models;
using Notification.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(NotificationMessage value)
        {
            await notificationService.SendNotificationAsync(value);
            return Ok();
        }
    }
}
