using System.Text;
using MediatR;
using Shope.Application.Base.Notifications;
using Shope.Application.Base.Reports;

namespace Shope.Application.Events;

public class OrderConfirmedReportEventHandler(IReportService reportService)
    : INotificationHandler<OrderConfirmedEvent>
{
    public async Task Handle(OrderConfirmedEvent notification, CancellationToken cancellationToken)
    {
        // Create a content to register a report of the order confirmation

        var content = new StringBuilder();
        content.AppendLine($"Order Id: {notification.OrderId}");
        content.AppendLine($"Order Creation Date: {notification.OrderCreatedAt}");
        content.AppendLine($"Order Status: {notification.OrderStatus}");
        content.AppendLine($"Customer Id: {notification.CustomerId}");
        content.AppendLine($"Customer Name: {notification.CustomerName}");
        content.AppendLine($"Customer E-mail: {notification.CustomerEmail}");
        content.AppendLine("Order Items:");
        foreach (var item in notification.OrderItems)
        {
            content.AppendLine($"- Id: {item.Id}");
            content.AppendLine($"  ProductId: {item.ProductId}");
            content.AppendLine($"  Quantity: {item.Quantity}");
        }

        await reportService.RegisterReportAsync("New Order Confirmed", content.ToString());
    }
}
