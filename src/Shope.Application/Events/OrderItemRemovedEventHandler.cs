using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shope.Application.Base.Database;

namespace Shope.Application.Events;

public class OrderItemRemovedEventHandler(IShopeeContext context, ILogger<OrderItemRemovedEventHandler> logger) 
    : INotificationHandler<OrderItemRemovedEvent>
{
    public async Task Handle(OrderItemRemovedEvent notification, CancellationToken cancellationToken)
    {
        // Get product and increase his stock

        var productId = notification.ProductId;
        var quantityToIncrease = notification.ProductQuantity;

        var product = await context.Products.FirstOrDefaultAsync(p => 
            p.Id == productId, cancellationToken);

        if (product is null)
            throw new Exception($"Product with id '{productId}' not found");

        var actualStock = product.Stock;

        product.IncreaseStock(quantityToIncrease);

        var newStock = product.Stock;

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            """
            The stock of product with id '{productId}' has been 
            increased from '{actualStock}' to '{newStock}'
            """,
            productId, actualStock, newStock
            );
    }
}
