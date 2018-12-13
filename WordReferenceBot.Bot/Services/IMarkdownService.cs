using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Bot.Services
{
    public interface IMarkdownService
    {
        string FormatTranslation(Word word);
    }
}
