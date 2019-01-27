using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using TinyScraper.Abstractions;

namespace TinyScraper
{
    public static class DispatcherFactory
    {
        private static int _maxDegreeOfParallelism = 5;
        private static Dispatcher _dispatcher;

        public static void SetMaxDegreeOfParallelism(int maxDegreeOfParallelism)
        {
            if (_dispatcher != null && _dispatcher.IsRunning())
                throw new FieldAccessException("Cannot change MaxDegreeOfParallelism while running.");

            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        public static void SetDispatcher(Dispatcher dispatcher)
        {
            if (_dispatcher != null && _dispatcher.IsRunning())
                throw new FieldAccessException("Cannot change Dispatcher while running.");

            _dispatcher = dispatcher;
        }

        public static Dispatcher GetDispatcher(ILogger logger, CancellationToken ct = default(CancellationToken))
        {
            return _dispatcher ?? (_dispatcher = new InMemoryDispatcher(_maxDegreeOfParallelism, logger, ct));
        }
    }
}
