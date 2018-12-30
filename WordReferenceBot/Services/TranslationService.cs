using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordreferenceBot.Scraper;
using WordReferenceBot.Domain.Entities;

namespace WordReferenceBot.Api.Services
{
    public class TranslationService : ITranslationService
    {
        private TranslationExtractor _extractor;
        public TranslationService(IWordReferenceRequest request)
        {
            _extractor = new TranslationExtractor(request);
        }
        public async Task<Word> Translate(string word)
        {
            return await _extractor.ExtractTranslation(word);

        }
    }
}
