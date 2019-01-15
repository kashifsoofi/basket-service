using System.Collections.Generic;
using Checkout.BasketService.Models;
using Checkout.BasketService.Services.Results;
using Checkout.BasketService.Stores;

namespace Checkout.BasketService.Services
{
    public class CustomerBasketService : IBasketService
    {
        private readonly IBasketStore _basketStore;

        public CustomerBasketService(IBasketStore basketStore)
        {
            _basketStore = basketStore;
        }

        public Basket GetBasketByCustomerId(string customerId)
        {
            return _basketStore.FindByCustomerId(customerId);
        }

        public CreateBasketResult CreateBasketForCustomer(string customerId)
        {
            var basket = GetBasketByCustomerId(customerId);
            if (basket != null)
            {
                return CreateBasketResult.AlreadyExists;
            }

            _basketStore.Create(customerId);
            return CreateBasketResult.Created;
        }
    }
}
