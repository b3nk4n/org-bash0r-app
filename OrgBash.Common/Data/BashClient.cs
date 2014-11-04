using OrgBash.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrgBash.Common.Data
{
    public class BashClient : IBashClient
    {
        #region Members

        private const string BASE_SERVICE_URI = "http://bsautermeister.de/org-bash0r/api/service.php";
        private const string BASE_WEBSITE_URI = "http://bash.org/";

        private const string SERVICE_METHOD = "method";

        private HttpClientHandler HttpClientHandler { get; set; }

        public CookieContainer Cookies
        {
            get { return HttpClientHandler.CookieContainer; }
            set { HttpClientHandler.CookieContainer = value; }
        }

        private HttpClient _httpClient = new HttpClient();

        #endregion

        #region Constructors

        public BashClient()
        {
            HttpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
            _httpClient = new HttpClient(HttpClientHandler);
        }

        #endregion

        #region Public Methods

        public async Task<BashCollection> GetQuotesAsync(string order)
        {
            string uriString = String.Format("{0}?{1}={2}",
                BASE_SERVICE_URI, 
                SERVICE_METHOD, order);
            var response = await _httpClient.GetAsync(uriString);
            
            if (response.IsSuccessStatusCode)
            {
                var encodedData = await ReadEncodedContentAsync(response);
                try
                {
                    return JsonConvert.DeserializeObject<BashCollection>(encodedData);
                }
                catch (JsonReaderException)
                {
                    return null; // TODO: find reason for parsing errors.
                }

            }

            return null;
        }

        public async Task<BashCollection> GetQueryAsync(string term)
        {
            // TODO: escape "term" string?

            string uriString = String.Format("{0}?{1}={2}&{3}={4}",
                BASE_SERVICE_URI, 
                SERVICE_METHOD, AppConstants.METHOD_SEARCH,
                AppConstants.PARAM_TERM, term);
            var response = await _httpClient.GetAsync(uriString);

            if (response.IsSuccessStatusCode)
            {
                var encodedData = await ReadEncodedContentAsync(response);
                try
                {
                    return JsonConvert.DeserializeObject<BashCollection>(encodedData);
                }
                catch (JsonReaderException)
                {
                    return null; // TODO: find reason for parsing errors.
                }
            }

            return null;
        }

        public async Task<bool> RateAsync(int id, string type, string linkParam)
        {
            string uriString = String.Format("{0}?{1}",
                BASE_WEBSITE_URI, linkParam);

            var response = await _httpClient.GetAsync(uriString);

            if (response.IsSuccessStatusCode)
            {
                var encodedData = await ReadEncodedContentAsync(response);

                if (encodedData != null)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        private static async Task<string> ReadEncodedContentAsync(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsByteArrayAsync();
            string encodedString = Encoding.GetEncoding("iso-8859-1").GetString(data, 0, data.Length);
            return encodedString;
        }

        #endregion
    }
}
