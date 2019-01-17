using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.BasketService.Sdk.Exceptions;
using Newtonsoft.Json;

namespace Checkout.BasketService.Sdk
{
    public interface IApiClient
    {
        Task<TResult> GetAsync<TResult>(string path);
        Task<TResult> PostAsync<TRequest, TResult>(string path, TRequest request) where TRequest : class;
        Task<TResult> PutAsync<TRequest, TResult>(string path, TRequest request) where TRequest : class;
        Task<TResult> DeleteAsync<TResult>(string path);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(BasketServiceOptions options)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(options.Uri)
            };
        }

        public async Task<TResult> GetAsync<TResult>(string path)
        {
            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();

            return await DeserializeJsonAsync<TResult>(response);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string path, TRequest request) where TRequest : class
        {
            var content = CreateHttpContent(request);
            var response = await _httpClient.PostAsync(path, content);
            response.EnsureSuccessStatusCode();

            return await DeserializeJsonAsync<TResult>(response);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string path, TRequest request) where TRequest : class
        {
            var content = CreateHttpContent(request);
            var response = await _httpClient.PutAsync(path, content);
            response.EnsureSuccessStatusCode();

            return await DeserializeJsonAsync<TResult>(response);
        }

        public async Task<TResult> DeleteAsync<TResult>(string path)
        {
            var response = await _httpClient.DeleteAsync(path);
            response.EnsureSuccessStatusCode();

            return await DeserializeJsonAsync<TResult>(response);
        }

        private void ValidateResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode != HttpStatusCode.NotFound)
                    throw new ResourceNotFoundException(response.StatusCode);

                throw new ApiException(response.StatusCode);
            }
        }

        private HttpContent CreateHttpContent<T>(T content) where T : class
        {
            if (content == null)
                return null;

            var httpContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            return httpContent;
        }

        private async Task<T> DeserializeJsonAsync<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }
}
