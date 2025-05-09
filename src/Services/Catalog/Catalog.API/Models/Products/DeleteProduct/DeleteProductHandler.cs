namespace Catalog.API.Models.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler(
    IDocumentSession session,
    ILogger<DeleteProductHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting product {Id}", command.Id);
        
        // Check if the product exists
        var existingProduct = await session.LoadAsync<Product>(command.Id);
        if (existingProduct is null)
        {
            logger.LogWarning("Product {Id} not found", command.Id);
            return new DeleteProductResult(false);
        }
        
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}