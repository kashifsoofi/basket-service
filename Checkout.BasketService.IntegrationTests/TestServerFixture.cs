using System;
using System.Collections.Generic;
using System.Net.Http;
using Checkout.BasketService.Models;
using Checkout.BasketService.Stores;
using Checkout.BasketService.Stores.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.BasketService.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }
        public List<Basket> Baskets { get; }

        public TestServerFixture()
        {
            Baskets = new List<Basket>
            {
                new Basket("ExistingTestClientId", new List<Item>()),
                new Basket("TestClientIdWithMulitpleItems1", new List<Item> { new Item("Item1", 2), new Item("Item2", 4), new Item("Item3", 3)}),
                new Basket("TestClientIdWithMulitpleItems2", new List<Item> { new Item("Item1", 2), new Item("Item2", 4), new Item("Item3", 3)}),
                new Basket("TestClientIdWithSingleItem", new List<Item> { new Item("Item1", 5) })
            };

            var builder = new WebHostBuilder()
                .UseSolutionRelativeContentRoot("Checkout.BasketService")
                .UseEnvironment("Development")
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IBasketStore, InMemoryBasketStore>(_ => new InMemoryBasketStore(Baskets));
                })
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
