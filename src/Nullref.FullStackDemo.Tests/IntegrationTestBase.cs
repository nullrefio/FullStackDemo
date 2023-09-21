using Nullref.FullStackDemo.API;
using Xunit.Abstractions;

namespace Nullref.FullStackDemo.Tests
{
    public class IntegrationTestBase
    {
        protected const string BASE_API_URL = "api/v1/";
        private static readonly Dictionary<string, HttpClient> _clientHashCache = new();
        private static readonly object _locker = new();

        protected IntegrationTestBase(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
        {
            lock (_locker)
            {
                var s = this.GetType().FullName;
                if (!_clientHashCache.ContainsKey(s))
                {
                    var _client = factory
                        .WithWebHostBuilder(builder =>
                        {
                        })
                        .CreateClient();
                    _clientHashCache.Add(s, _client);
                }
            }
        }
    }
}
