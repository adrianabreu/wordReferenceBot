using HtmlAgilityPack;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WordreferenceBot.Scraper;
using WordReferenceBot.Domain;
using WordReferenceBot.Domain.Entities;
using Xunit;

namespace WordReferenceBot.Core.Test
{
    public class TranslationExtractorTest
    {
        private Mock<IWordReferenceRequest> _mockRequest;
        private TranslationExtractor _extractor;
        public TranslationExtractorTest()
        {
            var htmlResource = TestResources.beautiful;

            var htmlPage = new HtmlDocument();
            htmlPage.LoadHtml(htmlResource);

            _mockRequest = new Mock<IWordReferenceRequest>();
            _mockRequest.Setup(r => r.RequestTranslation(It.IsAny<string>())).ReturnsAsync(htmlPage);

            _extractor = new TranslationExtractor(_mockRequest.Object);
        }
        [Fact]
        public async Task Given_TranslationsNodes_Should_Parse_Them()
        {
            var word = "beautiful";
            var expectedTranslations = JsonConvert.DeserializeObject<Word>(TestResources.beautifultranslations);

            var result = await _extractor.ExtractTranslation(word);

            Assert.Equal(expectedTranslations.Value, result.Value);

            for (int index = 0; index < expectedTranslations.Translations.Count(); index++)
            {
                Assert.Equal(expectedTranslations.Translations.ElementAt(index).WordExpression, result.Translations.ElementAt(index).WordExpression);

                Assert.True(expectedTranslations.Translations.ElementAt(index).Meanings.SequenceEqual(result.Translations.ElementAt(index).Meanings));
                Assert.True(expectedTranslations.Translations.ElementAt(index).PossibleTranslations.SequenceEqual(result.Translations.ElementAt(index).PossibleTranslations));
            }
            
        }
    }
}
