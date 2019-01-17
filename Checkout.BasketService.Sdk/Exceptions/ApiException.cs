using System;
using System.Net;

namespace Checkout.BasketService.Sdk.Exceptions
{
    public class ApiException : Exception
    {
        protected HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
