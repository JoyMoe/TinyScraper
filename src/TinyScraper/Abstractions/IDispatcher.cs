using System;
using System.Threading;
using System.Threading.Tasks;

namespace TinyScraper.Abstractions
{
    public interface IDispatcher
    {
        Task<TResult> RunAsync<TResult>(Func<TResult> task, CancellationToken ct = default(CancellationToken));

        bool IsRunning();
    }
}
