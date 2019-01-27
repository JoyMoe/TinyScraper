using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TinyScraper.Abstractions;

namespace TinyScraper
{
    public class Scraper<T> : IDisposable
    {
        private readonly string _url;

        private readonly HashSet<Parser<T>> _parsers = new HashSet<Parser<T>>();

        private readonly HashSet<Pipeline<T>> _pipelines = new HashSet<Pipeline<T>>();

        private readonly ILogger _logger;

        private readonly Dispatcher _dispatcher;

        private readonly CancellationToken _cancellationToken;

        private HttpContent _content;

        private HttpMethod _method;

        private CookieContainer _cookies;

        private IWebProxy _proxy;

        public HttpResponseMessage Response { get; private set; }

        public HashSet<T> Items { get; } = new HashSet<T>();

        public Scraper(string url,
            HttpContent content = null,
            HttpMethod method = null,
            CookieContainer cookies = null,
            IWebProxy proxy = null,
            ILogger logger = null,
            Dispatcher dispatcher = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _url = url;
            _content = content;
            _method = method ?? HttpMethod.Get;
            _proxy = proxy;
            _cookies = cookies;

            if (logger == null)
            {
                var loggerFactory = new LoggerFactory().AddConsole();

                _logger = loggerFactory.CreateLogger<Scraper<T>>();
            }
            else
            {
                _logger = logger;
            }

            _dispatcher = dispatcher ?? DispatcherFactory.GetDispatcher(_logger, cancellationToken);

            _cancellationToken = cancellationToken;
        }

        public void Dispose()
        {
            //
        }

        public static Scraper<T> Create(string url,
            HttpContent content = null,
            HttpMethod method = null,
            CookieContainer cookies = null,
            IWebProxy proxy = null,
            ILogger logger = null,
            Dispatcher dispatcher = null,
            CancellationToken cancellationToken = default(CancellationToken))
            => new Scraper<T>(url, content, method, cookies, proxy, logger, dispatcher, cancellationToken);

        public Scraper<T> SetContent(HttpContent content)
        {
            _content = content;
            return this;
        }

        public Scraper<T> SetMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public Scraper<T> SetCookieContainer(CookieContainer cookies)
        {
            _cookies = cookies;
            return this;
        }

        public Scraper<T> SetProxy(IWebProxy proxy)
        {
            _proxy = proxy;
            return this;
        }

        public Scraper<T> AddParser(Parser<T> parser)
        {
            parser.Logger = _logger;
            _parsers.Add(parser);
            return this;
        }

        public Scraper<T> AddPipeline(Pipeline<T> pipeline)
        {
            pipeline.Logger = _logger;
            _pipelines.Add(pipeline);
            return this;
        }

        public async Task<Scraper<T>> FetchAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
            {
                cancellationToken = _cancellationToken;
            }

            var handler = new HttpClientHandler
            {
                CookieContainer = _cookies,
                Proxy = _proxy
            };

            var request = new HttpRequestMessage
            {
                Content = _content,
                Method = _method,
                RequestUri = new Uri(_url)
            };

            _logger.LogInformation($"Schedule to fetch {request}.");

            var task = await _dispatcher.RunAsync(async () =>
            {
                _logger.LogInformation($"Start to fetch {request}.");

                using (var client = new HttpClient(handler))
                {
                    return await client.SendAsync(request, cancellationToken);
                }
            }, cancellationToken);

            Response = await task;

            _logger.LogInformation($"Succeed in fetching {request}.");

            return this;
        }

        public async Task<Scraper<T>> ParseAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
            {
                cancellationToken = _cancellationToken;
            }

            if (_parsers.Count == 0)
                throw new ArgumentNullException(nameof(_parsers), $"Add a parser with {nameof(AddParser)}");

            foreach (var parser in _parsers)
            {
                try
                {
                    Items.AddRange(await parser.ParseAsync(Response, cancellationToken));
                }
                catch (NotImplementedException)
                {
                    Items.AddRange(parser.Parse(Response));
                }
            }

            return this;
        }

        public async Task<Scraper<T>> ProcessAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
            {
                cancellationToken = _cancellationToken;
            }

            foreach (var pipeline in _pipelines)
            {
                try
                {
                    await pipeline.ProcessAsync(Items, cancellationToken);
                }
                catch (NotImplementedException)
                {
                    pipeline.Process(Items);
                }
            }

            return this;
        }

        public async Task<Scraper<T>> ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
            {
                cancellationToken = _cancellationToken;
            }

            return await (await (await FetchAsync(cancellationToken)).ParseAsync(cancellationToken)).ProcessAsync(cancellationToken);
        }
    }
}
