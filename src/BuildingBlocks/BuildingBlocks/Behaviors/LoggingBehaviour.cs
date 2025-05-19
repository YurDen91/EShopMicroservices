using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehaviour<TRequest, TResponse>
    (ILogger<LoggingBehaviour<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handling request={Request} - Response={Response} - RequestData={RequstData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);
        
        var timer = new Stopwatch();
        timer.Start();
        
        var response = await next();
        
        timer.Stop();
        
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} milliseconds to process",
                typeof(TRequest).Name, timeTaken.Milliseconds);
        }
        
        logger.LogInformation("[END] Finished handling request={Request} - Response={Response} - TimeTaken={TimeTaken}",
            typeof(TRequest).Name, typeof(TResponse).Name, timeTaken.Milliseconds);
        
        return response;
    }
}