using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

/// <summary>
/// A pipeline behavior for validating requests in the MediatR pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
public class ValidationBehaviour<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    /// <summary>
    /// Handles the validation of the request before passing it to the next behavior or handler in the pipeline.
    /// </summary>
    /// <param name="request">The incoming request to be validated.</param>
    /// <param name="next">The delegate to invoke the next behavior or handler in the pipeline.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The response from the next behavior or handler in the pipeline.</returns>
    /// <exception cref="ValidationException">Thrown if any validation failures are detected.</exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context)));

        // Extract validation failures from the results.
        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();
        
        // If there are any validation failures, throw a ValidationException.
        if (failures.Any())
            throw new ValidationException(string.Join(Environment.NewLine, failures));
        
        return await next();
    }
}