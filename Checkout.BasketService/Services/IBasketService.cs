using Checkout.BasketService.Models;
using Checkout.BasketService.Services.Results;

namespace Checkout.BasketService.Services
{
    public interface IBasketService
    {
        Basket GetBasketByCustomerId(string customerId);
        CreateBasketResult CreateBasketForCustomer(string customerId);
    }
}
