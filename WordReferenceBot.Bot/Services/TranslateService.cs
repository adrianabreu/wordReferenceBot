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
    public class TranslateService : ITranslateService
    {
        private readonly IBotService _botService;
        private readonly IMarkdownService _markdownService;
        private readonly ILogger<TranslateService> _logger;
        private readonly HttpClient _httpClient;

        public TranslateService(IBotService botService, IMarkdownService markdownService, ILogger<TranslateService> logger)
        {
            _botService = botService;
            _markdownService = markdownService;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task TranslateAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }

            var message = update.Message;

            _logger.LogInformation("Received Message from {0}", message.Chat.Id);

            if (message.Type == MessageType.Text)
            {
                var wordsToTranslate = message.Text.Split(',');

                var tasks = wordsToTranslate.Select(x => _httpClient.GetAsync($"https://wordreferencebotapi.azurewebsites.net/api/translations/{x}"));
                var responses = await Task.WhenAll(tasks);
                var translations = await Task.WhenAll(responses.Select(r => r.Content.ReadAsAsync<Word>()));
                var formattedTranslations = translations.Select(t => _markdownService.FormatTranslation(t));
                var messageWithoutFormatting = String.Join(';', formattedTranslations);
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, messageWithoutFormatting, ParseMode.Markdown);
            }
        }
    }
}
