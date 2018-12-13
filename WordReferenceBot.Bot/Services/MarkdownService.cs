using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Bot.Services
{
    public class MarkdownService : IMarkdownService
    {
        public string FormatTranslation(Word word)
        {
            string formattedMessage = "";

            foreach (var translation in word.Translations)
            {
                formattedMessage += $"*{translation.WordExpression}*\n";
                foreach (var meaning in translation.Meanings)
                {
                    formattedMessage += $"_{meaning}_\n";
                }
                foreach (var possibleTranslation in translation.PossibleTranslations)
                {
                    formattedMessage += $"{possibleTranslation}\n";
                }
                formattedMessage += "-----------------------------------\n";
            }
            return formattedMessage;
        }
    }
}
