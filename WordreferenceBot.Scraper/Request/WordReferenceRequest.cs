using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordreferenceBot.Scraper
{
    public class WordReferenceRequest : IWordReferenceRequest
    {
        private readonly string _wordreferenceUrl;
        private HttpClient _httpClient;

        public WordReferenceRequest(string wordReferenceUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 Firefox/26.0");
            _wordreferenceUrl = wordReferenceUrl;
        }

        public async Task<HtmlDocument> RequestTranslation(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new Exception("Request word requires an entry param");
            }
            
            var response = await _httpClient.GetAsync(_wordreferenceUrl + word);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml( await response.Content.ReadAsStringAsync());

            return htmlDocument;
        }
    }
}
