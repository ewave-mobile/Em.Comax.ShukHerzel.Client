using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzel.Integration.services
{
    public class ComaxApiClient : IComaxApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationRepository _configService;
        private readonly IDatabaseLogger _databaseLogger;
        private readonly IApiConfigService _apiConfigService;

        public ComaxApiClient(HttpClient httpClient, IConfigurationRepository configurationRepository, IDatabaseLogger databaseLogger, IApiConfigService apiConfigService)
        {
            _httpClient = httpClient;
            _configService = configurationRepository;
            _databaseLogger = databaseLogger;
            _apiConfigService = apiConfigService;
            var configComax = _apiConfigService.GetApiConfig("COMAX");
            if (!string.IsNullOrEmpty(configComax.ComaxBaseUrl))
            {
                _httpClient.BaseAddress = new Uri(configComax.ComaxBaseUrl);
            }
        }

        public async Task<IList<CatalogItemDto>> GetCatalogItemsAsync(CancellationToken cancellationToken = default)
        {
            var configComax = _apiConfigService.GetApiConfig("COMAX"); // Fetch configuration within the scoped context

            // Configure HttpClient based on fetched configuration
            _httpClient.BaseAddress = new Uri(configComax.ComaxBaseUrl ?? "http://ws.comax.co.il/Comax_WebServices/");

            // Add headers if API key is provided
           

            var response = await _httpClient.GetAsync("catalog", cancellationToken);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CatalogItemDto>>(json);
        }
        public async Task<string> GetCatalogXmlAsync(Branch branch, DateTime lastUpdateDate, CancellationToken cancellationToken = default)
        {
            try
            {
             //   var configComax = _apiConfigService.GetApiConfig("COMAX"); // Fetch configuration within the scoped context

                // Configure HttpClient based on fetched configuration
          //      _httpClient.BaseAddress = new Uri(configComax.ComaxBaseUrl ?? "http://ws.comax.co.il/Comax_WebServices/");

                // Add headers if API key is provided
                // 1. Get your config row (assuming there's only one row or you're picking by CompanyID, etc.)
                var config = await _configService.getCompanyConfig(Constants.SHUK_HERZEL_COMPANY_ID);
                // or query from DB directly

                // 2. Build the full GET URL
                var url = BuildComaxCatalogUrl(config, branch, lastUpdateDate);

                // 3. Make the GET request
                var response = await _httpClient.GetAsync(url, cancellationToken);
                await _databaseLogger.LogTraceAsync(_httpClient.BaseAddress?.ToString(), url, "",response.StatusCode.ToString());
                response.EnsureSuccessStatusCode();

                // 4. Read the raw XML string
                var xml = await response.Content.ReadAsStringAsync();
                return xml;
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("COMAX_API_CLIENT", "GetCatalogXmlAsync", ex);
                return "";
            }
        }

        public async Task<IList<PromotionDto>> GetPromotionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("promotions", cancellationToken);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<PromotionDto>>(json);
        }
        // Suppose we have a method that builds the query string
       

        public string BuildComaxCatalogUrl(Configuration config, Branch branch, DateTime LastUpdateDate)
        {

            // Example base URL: "https://ws.comax.co.il/Comax_WebServices/Items_Service.asmx/Get_AllItemsDetailsBySearch"
            // We'll append query params from the config object
            // Note: You might need to handle null or empty strings carefully, or pass them as blank.

           // var baseUrl = config.ComaxApiUrl; // e.g. "https://ws.comax.co.il/Comax_WebServices/Items_Service.asmx/Get_AllItemsDetailsBySearch"
            var endpoint = "Items_Service.asmx/Get_AllItemsDetailsBySearch"; // or whatever your endpoint is
            // Build the query string with each param
            // (In real code, use UriBuilder or HttpUtility to URL-encode your parameters properly)
            var query = $"?ItemID={config.ItemId}" +
                        $"&DepartmentID={config.DepartmentId}" +
                        $"&GroupID={config.GroupId}" +
                        $"&Sub_GroupID={config.SubGroupId}" +
                        $"&ItemModelID={config.ItemModelId}" +
                        $"&ItemColorID={config.ItemColorId}" +
                        $"&ItemSizeID={config.ItemSizeId}" +
                        $"&StoreID={branch.ComaxStoreId}" +
                        $"&PriceListID={branch.ComaxPriceListId}" + // or different if your system is set differently
                        $"&StoreIDForOpenOrdersOffset={config.StoreIdforOpenOrdersOffset}" +
                        $"&SupplierID={config.SupplierId}" +
                        $"&CustomerID={config.CustomerId}" +
                        $"&LastUpdatedFromDate={LastUpdateDate.ToString("dd/MM/yyyy HH:mm:ss")}" +
                        // or config.PromotionLastUpdatedDate if that’s your last update date?
                        $"&LoginID={config.LoginId}" +
                        $"&LoginPassword={config.LoginPassword}" +
                        $"&ShowInWeb={config.ShowInWeb?.ToString() ?? "False"}" +
                        $"&WithOutArchive={config.WithOutArchive?.ToString() ?? "False"}";

            // Combine
           // return baseUrl + query;
           return endpoint + query;
        }

        public async Task<PromotionDto> GetPromotionsAsync(
            Branch branch,
            DateTime lastUpdateDate,
            bool justActive = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var configComax = _apiConfigService.GetApiConfig("COMAX"); // Fetch configuration within the scoped context

                // Configure HttpClient based on fetched configuration
              //  _httpClient.BaseAddress = new Uri(configComax.ComaxBaseUrl ?? "http://ws.comax.co.il/Comax_WebServices/");
                // 1. Build request body (based on your sample)
                //    We assume Comax expects JSON with "Params", "LoginID", "LoginPassword"
                var config = await _configService.getCompanyConfig(Constants.SHUK_HERZEL_COMPANY_ID);
                // e.g. config.LoginId, config.LoginPassword, etc.

                var body = new
                {
                    Params = $"{{'LastUpdatedDate':'{lastUpdateDate:dd/MM/yyyy HH:mm:ss}','JustActive':{config.PromotionJustActive?.ToString().ToLower() ?? "false"}, 'StoreList':'{branch.ComaxStoreId}','PriceListID':'{branch.ComaxPriceListId}'}}",
                    LoginID = config.LoginId,
                    LoginPassword = config.LoginPassword
                };

                // 2. Post to Comax's "GetPromotionsDef" endpoint
                var jsonContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Promotions_Service.asmx/GetPromotionsDef", jsonContent, cancellationToken);
                await _databaseLogger.LogTraceAsync(_httpClient.BaseAddress?.ToString() + "Promotions_Service.asmx/GetPromotionsDef", JsonConvert.SerializeObject(body),"", response.StatusCode.ToString());
                response.EnsureSuccessStatusCode();

                // 3. Read the response (likely JSON)
                var rawJson = await response.Content.ReadAsStringAsync(cancellationToken);

                // 4. Deserialize to your model. 
                //    If the response is an array or an object with a property, 
                //    you might need a wrapper class. Adjust as needed.
                //    For example, if the result is just a direct list of "ClsPromotion", do this:
                var promotions = JsonConvert.DeserializeObject<PromotionDto>(rawJson);
                return promotions ?? new PromotionDto();
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("COMAX_API_CLIENT", "GetPromotionsAsync", ex);
                return new PromotionDto();
            }
        }
    }
}
