using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordreferenceBot.Scraper;
using WordReferenceBot.Api.Services;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationsController : ControllerBase
    {
        private ITranslationService _translationService;
        public TranslationsController(ITranslationService translationService)
        {
            _translationService = translationService;
        }
        [HttpGet("{word}")]
        public async Task<Word> GetAsync(string word)
        {
            // Check for stored results 

            // If not available, make the request
            return await _translationService.Translate(word);

            // Store the results in the database

            // Return the parsed result
            // return words;
        }
    }
}
