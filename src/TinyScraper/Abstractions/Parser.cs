using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TinyScraper.Abstractions
{
    public abstract class Parser<T> : IParser<T>, IDisposable
    {
        public ILogger Logger { get; set; }

        public void Dispose()
        {
            //
        }

        public virtual Task<IEnumerable<T>> ParseAsync(HttpResponseMessage body, CancellationToken cancellationToken = default(CancellationToken)) => throw new NotImplementedException();

        public virtual IEnumerable<T> Parse(HttpResponseMessage body) => throw new NotImplementedException();
    }
}
