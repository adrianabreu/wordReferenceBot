using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Api.Services
{
    public interface ITranslationService
    {
        Task<IEnumerable<Word>> Translate(string word);
    }
}
