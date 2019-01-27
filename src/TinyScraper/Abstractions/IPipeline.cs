using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TinyScraper.Abstractions
{
    public interface IPipeline<T>
    {
        Task ProcessAsync(HashSet<T> items, CancellationToken cancellationToken = default(CancellationToken));

        void Process(HashSet<T> items);
    }
}
