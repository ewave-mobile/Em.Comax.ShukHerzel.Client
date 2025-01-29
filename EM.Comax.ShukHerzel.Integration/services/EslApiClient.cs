// EslApiClient.cs
using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.DtoModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EM.Comax.ShukHerzel.Integration.services
{
    public class EslApiClient : IEslApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IApiConfigService _apiConfigService;

        public EslApiClient(HttpClient httpClient, IApiConfigService apiConfigService)
        {
            _httpClient = httpClient;
            _apiConfigService = apiConfigService;
            var apiConfig = _apiConfigService.GetApiConfig("EslApi");
            if (!string.IsNullOrEmpty(apiConfig.EslBaseUrl))
            {
                _httpClient.BaseAddress = new Uri(apiConfig.EslBaseUrl);
            }

            if (!string.IsNullOrEmpty(apiConfig.EslApiKey))
            {
                // Add it once if all requests use it
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", apiConfig.EslApiKey);
            }
        }

        /// <summary>
        /// Sends a batch of items to the ESL API for a specific store.
        /// </summary>
        /// <param name="storeId">The ID of the store.</param>
        /// <param name="items">The list of EslDto items to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendItemsToEslAsync(string storeId, IEnumerable<EslDto> items, CancellationToken cancellationToken = default)
        {
            // Get the ESL API configuration
            //var apiConfig = _apiConfigService.GetApiConfig("EslApi");
            //_httpClient.BaseAddress = new Uri(apiConfig.EslBaseUrl ?? "https://api-eu.vusion.io/vcloud/v1/stores/" );

            //// Add headers if API key is provided
            //if (!string.IsNullOrEmpty(apiConfig.EslApiKey))
            //{
            //    _httpClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key"); // Remove if already exists to prevent duplication
            //    _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiConfig.EslApiKey);
            //}

            // Construct the endpoint with the storeId
            string endpoint = $"{storeId}/items"; // e.g., "12345/items"

            // Send the POST request with the items as JSON
            var response = await _httpClient.PostAsJsonAsync(endpoint, items, cancellationToken);

            // Ensure the request was successful; throw an exception if not
            response.EnsureSuccessStatusCode();

            // Optionally, handle the response content if needed
            // var responseContent = await response.Content.ReadAsStringAsync();
            // Process responseContent as required
        }
    }
}
