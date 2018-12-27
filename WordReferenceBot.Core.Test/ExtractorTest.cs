using HtmlAgilityPack;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WordreferenceBot.Scraper;
using WordReferenceBot.Domain;
using Xunit;

namespace WordReferenceBot.Core.Test
{
    public class ExtractorTest
    {
        private Mock<IRequest> _mockRequest;
        private Extractor _extractor;
        public ExtractorTest()
        {
            var htmlResource = TestResources.beautiful;

            var htmlPage = new HtmlDocument();
            htmlPage.LoadHtml(htmlResource);

            _mockRequest = new Mock<IRequest>();
            _mockRequest.Setup(r => r.ResquestWord(It.IsAny<string>())).ReturnsAsync(htmlPage);

            _extractor = new Extractor(_mockRequest.Object);
        }
        [Fact]
        public async Task Given_TranslationsNodes_Should_Parse_Them()
        {
            var word = "beautiful";
            var expectedTranslations = JsonConvert.DeserializeObject<Word>(TestResources.beautifultranslations);


            var result = await _extractor.Extract(word);

            Assert.Equal(expectedTranslations.Value, result.Value);
            for(int index = 0; index < expectedTranslations.Translations.Count(); index++)
            {
                Assert.Equal(expectedTranslations.Translations.ElementAt(index).WordExpression, result.Translations.ElementAt(index).WordExpression);

                for (int j = 0; j < expectedTranslations.Translations.ElementAt(index).Meanings.Count(); j++)
                {
                    Assert.Equal(expectedTranslations.Translations.ElementAt(index).Meanings.ElementAt(j), result.Translations.ElementAt(index).Meanings.ElementAt(j));
                }

                for (int k = 0; k < expectedTranslations.Translations.ElementAt(index).PossibleTranslations.Count(); k++)
                {
                    Assert.Equal(expectedTranslations.Translations.ElementAt(index).PossibleTranslations.ElementAt(k), result.Translations.ElementAt(index).PossibleTranslations.ElementAt(k));
                }
            }
            
        }
    }
}
