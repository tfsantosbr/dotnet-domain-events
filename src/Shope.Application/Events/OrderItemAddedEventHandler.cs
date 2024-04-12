using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shope.Application.Base.Database;

namespace Shope.Application.Events;

public class OrderItemAddedEventHandler(IShopeeContext context, ILogger<OrderItemAddedEventHandler> logger) 
    : INotificationHandler<OrderItemAddedEvent>
{
    public async Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
    {
        // Get product and decress his stock
        
        var productId = notification.ProductId;
        var quantityToDecrease = notification.ProductQuantity;

        var product = await context.Products.FirstOrDefaultAsync(p => 
            p.Id == productId, cancellationToken);

        if (product is null)
            throw new Exception($"Product with id {productId} not found");
        
        var actualStock = product.Stock;

        product.DecreaseStock(quantityToDecrease);

        var newStock = product.Stock;

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            """
            The stock of product with id {productId} has been 
            decreased from {actualStock} to {newStock}
            """,
            productId, actualStock, newStock
            );
    }
}
