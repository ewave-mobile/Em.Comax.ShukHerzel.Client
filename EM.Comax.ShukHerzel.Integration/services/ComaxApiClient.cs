using EM.Comax.ShukHerzel.Integration.interfaces;
using EM.Comax.ShukHerzel.Models.DtoModels;
using EM.Comax.ShukHerzel.Models.Interfaces;
using EM.Comax.ShukHerzel.Models.Models;
using EM.Comax.ShukHerzl.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

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

        /// <summary>
        /// Gets catalog items for specific barcodes from Comax API
        /// </summary>
        public async Task<IList<CatalogItemDto>> GetCatalogItemsByBarcodesAsync(Branch branch, IEnumerable<string> barcodes, CancellationToken cancellationToken = default)
        {
            try
            {
                var config = await _configService.getCompanyConfig(branch.CompanyId??Constants.SHUK_HERZEL_COMPANY_ID);
                var barcodesArray = barcodes.ToArray();
                
                // Build the URL with the barcodes as a comma-separated list in the ItemID parameter
                var barcodesString = string.Join(",", barcodesArray);
                var lastUpdateDate = DateTime.Now.AddYears(-1); // Use a date far in the past to ensure we get all items
                
                // Modify the URL to include the barcodes
                var endpoint = ComaxConstants.CATALOG_METHOD_URL;
                var query = $"?ItemID={barcodesString}" +
                            $"&DepartmentID={config.DepartmentId}" +
                            $"&GroupID={config.GroupId}" +
                            $"&Sub_GroupID={config.SubGroupId}" +
                            $"&ItemModelID={config.ItemModelId}" +
                            $"&ItemColorID={config.ItemColorId}" +
                            $"&ItemSizeID={config.ItemSizeId}" +
                            $"&StoreID={branch.ComaxStoreId}" +
                            $"&PriceListID={branch.ComaxPriceListId}" +
                            $"&StoreIDForOpenOrdersOffset={config.StoreIdforOpenOrdersOffset}" +
                            $"&SupplierID={config.SupplierId}" +
                            $"&CustomerID={config.CustomerId}" +
                            $"&LastUpdatedFromDate={lastUpdateDate.ToString("dd/MM/yyyy HH:mm:ss")}" +
                            $"&LoginID={config.LoginId}" +
                            $"&LoginPassword={config.LoginPassword}" +
                            $"&ShowInWeb={config.ShowInWeb?.ToString() ?? "False"}" +
                            $"&WithOutArchive={config.WithOutArchive?.ToString() ?? "False"}" +
                            $"&SelByPriceList={config.SelByPriceList?.ToString() ?? "False"}";
                
                var url = endpoint + query;
                
                // Make the GET request
                var response = await _httpClient.GetAsync(url, cancellationToken);
                await _databaseLogger.LogTraceAsync(_httpClient.BaseAddress?.ToString(), url, "", response.StatusCode.ToString());
                response.EnsureSuccessStatusCode();
                
                // Read the response XML
                var xml = await response.Content.ReadAsStringAsync();
                
                // Parse the XML to extract catalog items
                // This is a simplified example - you would need to implement the actual XML parsing logic
                // based on the structure of the response from the Comax API
                var catalogItems = ParseCatalogItemsFromXml(xml);
                
                return catalogItems;
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("COMAX_API_CLIENT", "GetCatalogItemsByBarcodesAsync", ex);
                return new List<CatalogItemDto>();
            }
        }
        
        /// <summary>
        /// Gets catalog XML for specific barcodes from Comax API, handling barcode input in different ways
        /// </summary>
        public async Task<string> GetCatalogXmlForBarcodesAsync(Branch branch, IEnumerable<string> barcodes, bool useItemId = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var config = await _configService.getCompanyConfig(branch.CompanyId??Constants.SHUK_HERZEL_COMPANY_ID);
                var barcodesArray = barcodes.ToArray();
                var barcodesString = string.Join(",", barcodesArray);
                var lastUpdateDate = DateTime.Now.AddYears(-1); // Use a date far in the past to ensure we get all items
                
                // Modify the URL to include the barcodes
                var endpoint = ComaxConstants.CATALOG_METHOD_URL;
                var query = "";
                
                if (useItemId)
                {
                    // Put barcodes in ItemID parameter
                    query = $"?ItemID={barcodesString}" +
                            $"&DepartmentID={config.DepartmentId}" +
                            $"&GroupID={config.GroupId}" +
                            $"&Sub_GroupID={config.SubGroupId}" +
                            $"&ItemModelID={config.ItemModelId}" +
                            $"&ItemColorID={config.ItemColorId}" +
                            $"&ItemSizeID={config.ItemSizeId}";
                }
                else
                {
                    // Put barcodes in config object
                    query = $"?ItemID={config.ItemId}" +
                            $"&DepartmentID={config.DepartmentId}" +
                            $"&GroupID={config.GroupId}" +
                            $"&Sub_GroupID={config.SubGroupId}" +
                            $"&ItemModelID={config.ItemModelId}" +
                            $"&ItemColorID={config.ItemColorId}" +
                            $"&ItemSizeID={config.ItemSizeId}" +
                            $"&Barcode={barcodesString}";
                }

                // Add common parameters
                query += $"&StoreID={branch.ComaxStoreId}" +
                         $"&PriceListID={branch.ComaxPriceListId}" +
                         $"&StoreIDForOpenOrdersOffset={config.StoreIdforOpenOrdersOffset}" +
                         $"&SupplierID={config.SupplierId}" +
                         $"&CustomerID={config.CustomerId}" +
                         $"&LastUpdatedFromDate={lastUpdateDate.ToString("dd/MM/yyyy HH:mm:ss")}" +
                         $"&LoginID={config.LoginId}" +
                         $"&LoginPassword={config.LoginPassword}";
                if(config.ShowInWeb != null)
                {
                    query += $"&ShowInWeb={config.ShowInWeb?.ToString() ?? "False"}";
                }

                 query += $"&WithOutArchive={config.WithOutArchive?.ToString() ?? "False"}" +
                         $"&SelByPriceList={config.SelByPriceList?.ToString() ?? "False"}";
                
                var url = endpoint + query;
                
                // Make the GET request
                var response = await _httpClient.GetAsync(url, cancellationToken);
                await _databaseLogger.LogTraceAsync(_httpClient.BaseAddress?.ToString(), url, "", response.StatusCode.ToString());
                response.EnsureSuccessStatusCode();
                
                // Read the response XML
                var xml = await response.Content.ReadAsStringAsync();
                return xml;
            }
            catch (Exception ex)
            {
                await _databaseLogger.LogErrorAsync("COMAX_API_CLIENT", "GetCatalogXmlForBarcodesAsync", ex);
                return "";
            }
        }
        
        /// <summary>
        /// Parses catalog items from XML response
        /// </summary>
        private List<CatalogItemDto> ParseCatalogItemsFromXml(string xml)
        {
            // This is a placeholder for the actual XML parsing logic
            // You would need to implement this based on the structure of the XML response from the Comax API
            // For now, we'll return an empty list
            return new List<CatalogItemDto>();
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
                var config = await _configService.getCompanyConfig(branch.CompanyId??Constants.SHUK_HERZEL_COMPANY_ID);
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


        public async Task<string> GetCatalogNewXmlAsync(Branch branch, DateTime lastUpdateDate, CancellationToken cancellationToken = default)
        {
            try
            {
                //   var configComax = _apiConfigService.GetApiConfig("COMAX"); // Fetch configuration within the scoped context

                // Configure HttpClient based on fetched configuration
                //      _httpClient.BaseAddress = new Uri(configComax.ComaxBaseUrl ?? "http://ws.comax.co.il/Comax_WebServices/");

                // Add headers if API key is provided
                // 1. Get your config row (assuming there's only one row or you're picking by CompanyID, etc.)
                var config = await _configService.getCompanyConfig(branch.CompanyId ?? Constants.SHUK_HERZEL_COMPANY_ID);
                // or query from DB directly

                //2. Get URL Getting catalogItems
               // var prevEndPoint = ComaxConstants.CATALOG_METHOD_URL_NEW;
               // var response2 = await _httpClient.PostAsync(prevEndPoint,null);
               // var url = response2.re

                // 2. Build the full GET URL XML
                var  res = await BuildComaxNewCatalogUrl(config, branch, lastUpdateDate);


                XDocument doc = XDocument.Parse(res);
                XNamespace ns = "http://ws.comax.co.il/Comax_WebServices/";

                var resultElement = doc
                    ?.Root
                    ?.Element("{http://schemas.xmlsoap.org/soap/envelope/}Body")
                    ?.Element(ns + "Get_AllItems_WithoutSupplierDetailsResponse")
                    ?.Element(ns + "Get_AllItems_WithoutSupplierDetailsResult");

                string url1 = resultElement?.Value?.Trim();

                //var url = ComaxConstants.CATALOG_METHOD_URL;

                // 4. Make the GET request
                var response = await _httpClient.GetAsync(url1);
                await _databaseLogger.LogTraceAsync(_httpClient.BaseAddress?.ToString(), url1, "", response.StatusCode.ToString());
                response.EnsureSuccessStatusCode();

                // 5. Read the raw XML string
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
            var endpoint = ComaxConstants.CATALOG_METHOD_URL; // or whatever your endpoint is
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
                        $"&LoginPassword={config.LoginPassword}";
            if (config.ShowInWeb?.ToString() != null )
            {
                query += $"&ShowInWeb={config.ShowInWeb?.ToString() ?? "False"}";
            }
           
            query += $"&WithOutArchive={config.WithOutArchive?.ToString() ?? "False"}" +
                $"&SelByPriceList={config.SelByPriceList?.ToString() ?? "False"}";

            // Combine
           // return baseUrl + query;
           return endpoint + query;
        }

        public async Task<string> BuildComaxNewCatalogUrl(Configuration config, Branch branch, DateTime LastUpdateDate)
        {
            var endpoint = ComaxConstants.COMAX_WS_NAMESPACE + ComaxConstants.CATALOG_METHOD_URL_NEW;
            try
            {

                var xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""

                               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""

                               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                <Get_AllItems_WithoutSupplierDetails xmlns=""http://ws.comax.co.il/Comax_WebServices/"">
                <Params>
                <Department>{config.DepartmentId}</Department>
                <Group>{config.GroupId}</Group>
                <Sub_Group></Sub_Group>
                <StoreID>{branch.ComaxStoreId}</StoreID>
                <PriceListID>{branch.ComaxPriceListId}</PriceListID>
                <LastUpdatedFromDate>{LastUpdateDate.ToString("dd/MM/yyyy HH:mm:ss")}</LastUpdatedFromDate>
                </Params>
                <LoginID>{config.LoginId}</LoginID>
                <LoginPassword>{config.LoginPassword}</LoginPassword>
                </Get_AllItems_WithoutSupplierDetails>
                </soap:Body>
                </soap:Envelope>";
                ////var xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                ////<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                ////               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                ////               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                ////  <soap:Body>
                ////    <Get_AllItems_WithoutSupplierDetails xmlns=""http://ws.comax.co.il/Comax_WebServices/"">
                ////      <Params>
                ////        <Department></Department>
                ////        <Group></Group>
                ////        <Sub_Group></Sub_Group>
                ////        <StoreID>2</StoreID>
                ////        <PriceListID>1</PriceListID>
                ////        <LastUpdatedFromDate>01/09/2025 08:00</LastUpdatedFromDate>
                ////      </Params>
                ////      <LoginID>digital123</LoginID>
                ////      <LoginPassword>digital2027</LoginPassword>
                ////    </Get_AllItems_WithoutSupplierDetails>
                ////  </soap:Body>
                ////</soap:Envelope>";

                using var client = new HttpClient();

                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                var response = await client.PostAsync(endpoint, content);

                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                // throw;
                return null;
            }


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
                var config = await _configService.getCompanyConfig(branch.CompanyId ?? Constants.SHUK_HERZEL_COMPANY_ID);
                // e.g. config.LoginId, config.LoginPassword, etc.

                var body = new
                {
                    Params = $"{{'LastUpdatedDate':'{lastUpdateDate:dd/MM/yyyy HH:mm:ss}','JustActive':{config.PromotionJustActive?.ToString().ToLower() ?? "false"}, 'StoreList':'{branch.PromotionStoreListId ?? 1}'}}",
                    LoginID = config.LoginId,
                    LoginPassword = config.LoginPassword
                };

                // 2. Post to Comax's "GetPromotionsDef" endpoint
                var jsonContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ComaxConstants.PROMOTION_METHOD_URL, jsonContent, cancellationToken);
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

        public async Task<List<ItemSalePriceDto>> GetNewPricesAsync(Branch branch, DateTime fromDate, CancellationToken cancellationToken = default)
        {
            // Assume you have fetched your configuration from _configService
            var config = await _configService.getCompanyConfig(branch.CompanyId??Constants.SHUK_HERZEL_COMPANY_ID);

            // Build the SOAP envelope
            var soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
               xmlns:soap=""{ComaxConstants.SOAP_ENVELOPE_NAMESPACE}"">
  <soap:Body>
    <{ComaxConstants.SOAP_ACTION_GET_ALL_ITEMS_PRICES} xmlns=""{ComaxConstants.COMAX_WS_NAMESPACE}"">
      <ItemID></ItemID>
      <DepartmentID></DepartmentID>
      <GroupID></GroupID>
      <Sub_GroupID></Sub_GroupID>
      <ItemModelID></ItemModelID>
      <ItemColorID></ItemColorID>
      <ItemSizeID></ItemSizeID>
      <StoreID>{branch.ComaxStoreId}</StoreID>
      <PriceListID>{branch.ComaxPriceListId}</PriceListID>
      <StoreIDForOpenOrdersOffset></StoreIDForOpenOrdersOffset>
      <SupplierID></SupplierID>
      <CustomerID></CustomerID>
      <LastUpdatedFromDate>{fromDate.ToString("dd/MM/yyyy HH:mm")}</LastUpdatedFromDate>
      <LoginID>{config.LoginId}</LoginID>
      <LoginPassword>{config.LoginPassword}</LoginPassword>
      <ChangesAfter></ChangesAfter>
    </{ComaxConstants.SOAP_ACTION_GET_ALL_ITEMS_PRICES}>
  </soap:Body>
</soap:Envelope>";

            var queryParameters = $"?op={ComaxConstants.SOAP_ACTION_GET_ALL_ITEMS_PRICES}" +
                         $"&SalesPriceArray={branch.SalesPriceArray}" +
                         $"&SelByPriceList={config.SelByPriceList.ToString().ToLower()}" +
                         $"&FromDate_UpdatePrice={fromDate.ToString("dd/MM/yyyy HH:mm")}";
            var requestUrl = ComaxConstants.PRICE_METHOD_URL + queryParameters;
            // Set up the HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(soapEnvelope, Encoding.UTF8, ComaxConstants.CONTENT_TYPE_XML)
            };

            // Optionally, add cookies or other headers here if required

            // Make the HTTP request
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseXml = await response.Content.ReadAsStringAsync(cancellationToken);

            // You can now use an XML parser or XmlSerializer to deserialize the response.
            // For example, using XmlSerializer:
            var serializer = new XmlSerializer(typeof(GetAllItemsPricesBySearchResultDto));
            using (var reader = new StringReader(ExtractInnerXml(responseXml))) // Extract the inner XML of the response if needed
            {
                if (serializer.Deserialize(reader) is GetAllItemsPricesBySearchResultDto result)
                {
                    return result.Items;
                }
            }
            return new List<ItemSalePriceDto>();
        }
        private string ExtractInnerXml(string soapResponse)
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(soapResponse);

            // Create a namespace manager for the SOAP envelope
            var nsManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            nsManager.AddNamespace("comax", ComaxConstants.COMAX_WS_NAMESPACE); // e.g., "http://ws.comax.co.il/Comax_WebServices/"

            // Navigate to the node that contains the actual result.
            // Assuming the response structure is:
            // <soap:Envelope>
            //   <soap:Body>
            //     <Get_AllItemsPricesBySearchResponse xmlns="http://ws.comax.co.il/Comax_WebServices/">
            //       <Get_AllItemsPricesBySearchResult> ... </Get_AllItemsPricesBySearchResult>
            //     </Get_AllItemsPricesBySearchResponse>
            //   </soap:Body>
            // </soap:Envelope>
            var resultNode = doc.SelectSingleNode("//soap:Body/comax:Get_AllItemsPricesBySearchResponse/comax:Get_AllItemsPricesBySearchResult", nsManager);
            if (resultNode != null)
                return resultNode.OuterXml;

            // If not found, return the whole response (or throw an exception)
            return soapResponse;
        }

    }
}
