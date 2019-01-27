using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TinyScraper.Abstractions
{
    public interface IParser<T>
    {
        Task<IEnumerable<T>> ParseAsync(HttpResponseMessage body, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<T> Parse(HttpResponseMessage body);
    }
}
