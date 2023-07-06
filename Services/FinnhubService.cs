using ServiceContracts;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public FinnhubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            Dictionary<string, object>? companyProfile = new Dictionary<string, object>();
            using(HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token=cc676uaad3i9rj8tb1s0")
                };

               HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader streamReader = new StreamReader(stream);
                string response = streamReader.ReadToEnd();
                companyProfile = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
            }

            if (companyProfile == null)
                throw new InvalidOperationException("No response received from Finn Hub");

            return companyProfile;
            //return null;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //create http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token=cc676uaad3i9rj8tb1s0") //URI includes the secret token
            };

            //send request
            HttpResponseMessage httpResponseMessage =  await httpClient.SendAsync(httpRequestMessage);

            //read response body
            string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

            //convert response body (from JSON into Dictionary)
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");

            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            //return response dictionary back to the caller
            return responseDictionary;
        }
    }
}