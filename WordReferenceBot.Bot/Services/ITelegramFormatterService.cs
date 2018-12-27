using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordReferenceBot.Api.Models;

namespace WordReferenceBot.Bot.Services
{
    public interface ITelegramFormatterService
    {
        IEnumerable<string> FormatTranslation(WordDto word);
    }
}
