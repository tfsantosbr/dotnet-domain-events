namespace Shope.Application.Base.Notifications;

public interface INotificationService
{
    Task SendNotificationAsync(string email, string subject, string message);
}
