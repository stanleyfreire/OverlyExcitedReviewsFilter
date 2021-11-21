using Newtonsoft.Json;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReviewScrapper.Services
{
    public class DataFetcherService : IDataFetcherService
    {
        public async Task<List<Review>> FetchData(string endpoint, int pages)
        {
            var url = String.Concat(endpoint, $"?pages={pages}");
            var reviews = new List<Review>();

            try
            {

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")); // only accept json responses
                httpClient.Timeout = new TimeSpan(0, 5, 0); //5min for longer requests

                HttpResponseMessage response = await httpClient.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    reviews = JsonConvert.DeserializeObject<List<Review>>(responseString);
                else
                    return null;

                return reviews;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
