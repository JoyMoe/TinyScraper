using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TinyScraper;
using TinyScraper.Abstractions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DispatcherFactory.SetMaxDegreeOfParallelism(3);

            using (var scraper = Scraper<Page>.Create("http://jandan.net"))
            {
                scraper.AddParser(new PageListParser())
                    .AddPipeline(new FetchPage())
                    .ExecuteAsync();
            }

            using (var scraper = Scraper<Page>.Create("http://jandan.net"))
            {
                scraper.AddParser(new PageListParser())
                    .AddPipeline(new FetchPage())
                    .ExecuteAsync();
            }

            using (var scraper = Scraper<Page>.Create("http://jandan.net"))
            {
                scraper.AddParser(new PageListParser())
                    .AddPipeline(new FetchPage())
                    .ExecuteAsync();
            }

            using (var scraper = Scraper<Page>.Create("http://jandan.net"))
            {
                scraper.AddParser(new PageListParser())
                    .AddPipeline(new FetchPage())
                    .ExecuteAsync();
            }

            using (var scraper = Scraper<Page>.Create("http://jandan.net"))
            {
                scraper.AddParser(new PageListParser())
                    .AddPipeline(new FetchPage())
                    .ExecuteAsync();
            }

            while (true)
            {
                //
            }
        }

        private class Page
        {
            public string Url { get; set; }

            public string Title { get; set; }
        }

        private class PageListParser : Parser<Page>
        {
            public override async Task<IEnumerable<Page>> ParseAsync(HttpResponseMessage body, CancellationToken ct = default(CancellationToken))
            {
                // Console.WriteLine(await body.Content.ReadAsStringAsync());

                return null;
            }
        }

        private class FetchPage : Pipeline<Page>
        {
            public override Task ProcessAsync(HashSet<Page> items, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }
        }
    }
}
