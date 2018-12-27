using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WordreferenceBot.Scraper;
using WordReferenceBot.Api.Models;
using WordReferenceBot.Api.Services;

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
        public async Task<WordDto> GetAsync(string word)
        {
            // Check for stored results 

            // If not available, make the request
            var translatedWord = await _translationService.Translate(word);
            return new WordDto()
            {
                Value = translatedWord.Value,
                Translations = translatedWord.Translations.Select(t => new TranslationDto()
                {
                    Meanings = t.Meanings,
                    PossibleTranslations = t.PossibleTranslations,
                    WordExpression = t.WordExpression
                })
            };

            // Store the results in the database

            // Return the parsed result
            // return words;
        }
    }
}
