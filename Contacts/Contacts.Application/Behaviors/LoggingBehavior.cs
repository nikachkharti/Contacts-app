using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Contacts.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request = {Request} - Response = {Resposne} - RequestData = {RequestData}", typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var respone = await next();

            timer.Stop();

            var timeTaken = timer.Elapsed;

            //If the request is greater than 3 seconds, then log a warning.
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("[PERFORMANCE] The request = {Request} took {TimeTaken}", typeof(TRequest).Name, timeTaken);
            }

            logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
            return respone;
        }

    }

}
