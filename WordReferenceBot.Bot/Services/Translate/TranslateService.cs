using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public class TranslateService : ITranslateService
    {
        private readonly IBotService _botService;
        private readonly ITelegramFormatterService _markdownService;
        private readonly ILogger<TranslateService> _logger;
        private readonly HttpClient _httpClient;

        private readonly string _apiUrl;

        public TranslateService(IOptions<BotConfiguration> botConfiguration, IBotService botService, ITelegramFormatterService markdownService, ILogger<TranslateService> logger)
        {
            _botService = botService;
            _markdownService = markdownService;
            _logger = logger;
            _httpClient = new HttpClient();
            _apiUrl = botConfiguration.Value.ApiUrl;
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
                var wordsToTranslate = message.Text;
                // Do not forget to sanitize the input

                var response = await _httpClient.GetAsync($"{_apiUrl}/api/translations/{wordsToTranslate}");
                var translations = await response.Content.ReadAsAsync<WordDto>();

                var formattedTranslations = _markdownService.FormatTranslation(translations);

                foreach (var formattedTranslation in formattedTranslations)
                {
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, formattedTranslation, ParseMode.Markdown);
                }
            }
        }
    }
}
