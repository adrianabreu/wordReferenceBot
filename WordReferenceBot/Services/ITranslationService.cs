using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Api.Services
{
    public interface ITranslationService
    {
        Task<Word> Translate(string word);
    }
}
