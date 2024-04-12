using MediatR;
using Shope.Application.Base.Notifications;

namespace Shope.Application.Events;

public class OrderConfirmedEventHandler(INotificationService notificationService) 
    : INotificationHandler<OrderConfirmedEvent>
{
    public async Task Handle(OrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        // Send a notification to customer
        // that the order has been confirmed
        
        var customerEmail = notification.Order.Customer.Email;
        var customerName = notification.Order.Customer.Name;
        var orderId = notification.Order.Id;
        
        await notificationService.SendNotificationAsync(
            customerEmail,
            "Order Confirmed",
            $"Dear, {customerName}. Your order with id {orderId} has been confirmed."
            );
    }
}
