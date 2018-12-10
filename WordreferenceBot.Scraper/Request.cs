using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordreferenceBot.Scraper
{
    public class Request
    {
        private const string _wordreferenceUrl = "http://www.wordreference.com/es/translation.asp?tranword=";
        private HttpClient _httpClient;

        public Request()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 Firefox/26.0");
        }

        public async Task<HtmlDocument> ResquestWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new Exception("Request word requires string");
            }
            
            var response = await _httpClient.GetAsync(_wordreferenceUrl + word);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml( await response.Content.ReadAsStringAsync());

            return htmlDocument;
        }

        // TODO: 
        // Scraper debería contenedor los métodos para extraer las listas de strings, y debería trabajar con un html directamente no debería conocer el servicio http.
        // Parser no es adecuado deberái ser mejor... ¿Formatter? Y preparar el mensaje en markdown al estilo de telegram
        // Quizás el proyecto debería llamarse Extractor y así juntar los dos
        // Haría falta un proyecto de Domain que tuviera los modelos para persistirlos en base de datos, el servicio que los orquesta.

    }
}
