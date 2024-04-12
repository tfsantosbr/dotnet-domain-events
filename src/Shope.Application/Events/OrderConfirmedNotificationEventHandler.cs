using MediatR;
using Shope.Application.Base.Notifications;

namespace Shope.Application.Events;

public class OrderConfirmedNotificationEventHandler(INotificationService notificationService) 
    : INotificationHandler<OrderConfirmedEvent>
{
    public async Task Handle(OrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        // Send a notification to customer
        // that the order has been confirmed
        
        var customerEmail = notification.CustomerEmail;
        var customerName = notification.CustomerName;
        var orderId = notification.OrderId;
        
        await notificationService.SendNotificationAsync(
            customerEmail,
            "Order Confirmed",
            $"Dear, '{customerName}'. Your order with id '{orderId}' has been confirmed."
            );
    }
}
