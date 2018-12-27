using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WordReferenceBot.Api.Models;
using WordReferenceBot.Bot.Services;
using Xunit;

namespace WordReferenceBot.Bot.Test
{
    public class TelegramFormatterServiceTest
    {
        private ITelegramFormatterService _messageService;
        public TelegramFormatterServiceTest()
        {
            _messageService = new TelegramFormatterService();
        }
        [Fact]
        public void When_No_Translations_Then_Return_Message()
        {
            var expectedMessage = "No translation found for *Julia*";

            var wordDto = new WordDto()
            {
                Value = "Julia",
                Translations = new List<TranslationDto>()
            };

            var messages = _messageService.FormatTranslation(wordDto);

            Assert.Equal(expectedMessage, messages.First());

        }

        [Fact]
        public void When_Translation_Is_Too_Big_Then_Split_Message()
        {
            
            var wordDto = JsonConvert.DeserializeObject<WordDto>(TestResource.testtranslations);

            var messages = _messageService.FormatTranslation(wordDto);
            foreach(var message in messages)
            {
                Assert.True(message.Length < 4096);
            }

        }
    }
}
