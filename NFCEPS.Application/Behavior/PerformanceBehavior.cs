using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace NFCEPS.Application.Behavior
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : TResponse<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellation)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMillisecond = _timer.ElapsedMilliseconds;

            if (elapsedMillisecond > 1500)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning("NFCEPS long running task: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    requestName, elapsedMillisecond, request);
            }

            return response;
        }
    }
}
