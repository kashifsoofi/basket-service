using System.Net;

namespace Checkout.BasketService.Sdk.Exceptions
{
    public class ResourceNotFoundException : ApiException
    {
        public ResourceNotFoundException(HttpStatusCode statusCode)
            : base(statusCode)
        {
        }
    }
}
