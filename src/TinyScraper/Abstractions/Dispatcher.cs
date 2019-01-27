using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TinyScraper.Abstractions
{
    public class Dispatcher : IDispatcher
    {
        private readonly ILogger _logger;
        private readonly CancellationToken _ct;

        public Dispatcher(ILogger logger, CancellationToken ct = default(CancellationToken))
        {
            _logger = logger;

            _ct = ct;
        }

        public virtual Task<TResult> RunAsync<TResult>(Func<TResult> task, CancellationToken ct = default(CancellationToken)) => throw new NotImplementedException();

        public virtual bool IsRunning() => throw new NotImplementedException();
    }
}
