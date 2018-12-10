using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordreferenceBot.Scraper;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Word>> GetAsync()
        {
            // Check for stored results 

            // Make the request
            var scraper = new Request();
            // Check for problemas handling the request 400 causes explicit fail
            var htmlPage = (await scraper.ResquestWord("beautiful"));

            // Parse the html page
            var words = Extractor.Extract(htmlPage);

            // Store the results in the database

            // Return the parsed result
            return words;
        }
    }
}
