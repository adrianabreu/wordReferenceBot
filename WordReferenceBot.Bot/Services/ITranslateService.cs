using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WordReferenceBot.Bot.Services
{
    public interface ITranslateService
    {
        Task TranslateAsync(Update update);
    }
}
