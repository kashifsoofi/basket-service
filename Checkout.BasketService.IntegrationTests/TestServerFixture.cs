using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Checkout.BasketService.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseSolutionRelativeContentRoot("Checkout.BasketService")
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
