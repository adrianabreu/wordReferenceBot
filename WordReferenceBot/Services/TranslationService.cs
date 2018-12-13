using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordreferenceBot.Scraper;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Api.Services
{
    public class TranslationService : ITranslationService
    {
        private Extractor _extractor;
        public TranslationService()
        {
            _extractor = new Extractor();
        }
        public async Task<Word> Translate(string word)
        {
            return await _extractor.Extract(word);
        }
        
    }
}
