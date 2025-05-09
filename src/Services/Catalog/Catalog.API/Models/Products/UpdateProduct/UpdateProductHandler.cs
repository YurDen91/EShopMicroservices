using Catalog.API.Exceptions;

namespace Catalog.API.Models.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) 
    :ICommand<UpdateProductResult>;

public record class UpdateProductResult(bool IsSuccess);

internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) 
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductCommand handler called with command: {@Command}", command);
        
        // Check if the product exists
        var existingProduct = await session.LoadAsync<Product>(command.Id);
        if (existingProduct is null)
        {
            throw new ProductNotFoundException();
        }
        
        existingProduct = command.Adapt<Product>();
        session.Update(existingProduct);

        await session.SaveChangesAsync();
        return new UpdateProductResult(true);
    }
}