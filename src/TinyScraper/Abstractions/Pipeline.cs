using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TinyScraper.Abstractions
{
    public abstract class Pipeline<T> : IPipeline<T>, IDisposable
    {
        public ILogger Logger { get; set; }

        public void Dispose()
        {
            //
        }

        public virtual Task ProcessAsync(HashSet<T> items, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

        public virtual void Process(HashSet<T> items) => throw new NotImplementedException();
    }
}
