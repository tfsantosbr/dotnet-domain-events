using Microsoft.Extensions.Logging;
using Shope.Application.Base.Notifications;

namespace Shope.Infrastructure.Notifications;

public class NotificationService(ILogger<NotificationService> logger)
    : INotificationService
{
    public async Task SendNotificationAsync(string email, string subject, string message)
    {
        logger.LogInformation(
            "Sending notification to '{email}' with subject '{subject}' and message '{message}'", 
            email, subject, message
            );

        await Task.CompletedTask;
    }
}
