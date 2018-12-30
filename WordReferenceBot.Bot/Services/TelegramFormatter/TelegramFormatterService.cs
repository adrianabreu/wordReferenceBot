using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordReferenceBot.Api.Models;
using WordReferenceBot.Domain;

namespace WordReferenceBot.Bot.Services
{
    public class TelegramFormatterService : ITelegramFormatterService
    {
        private const int TELEGRAM_MESSAGE_SIZE = 4096;

        public IEnumerable<string> FormatTranslation(WordDto word)
        {
            if (word.Translations.Count() == 0)
            {
                return GenerateNoTranslationsMessage(word.Value);
            }

            var formattedMessages = new List<string>();
            var tempMessage = ""; 
            foreach (var translation in word.Translations)
            {
                var formattedTranslation = $"*{translation.WordExpression}*\n";
                foreach (var meaning in translation.Meanings)
                {
                    formattedTranslation += $"_{meaning}_\n";
                }
                foreach (var possibleTranslation in translation.PossibleTranslations)
                {
                    formattedTranslation += $"{possibleTranslation}\n";
                }
                formattedTranslation += "-----------------------------------\n";

                var added = tempMessage + formattedTranslation;
                if (added.Length > TELEGRAM_MESSAGE_SIZE)
                {
                    formattedMessages.Add(tempMessage);
                    tempMessage = formattedTranslation;
                } else
                {
                    tempMessage += formattedTranslation;
                }
            }
            formattedMessages.Add(tempMessage);
            return formattedMessages;
        }

        private IEnumerable<string> GenerateNoTranslationsMessage(string word)
        {
            return new string[] { $"No translation found for *{word}*" };
        }
    }
}
