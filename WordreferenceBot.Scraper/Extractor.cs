using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordReferenceBot.Domain;

namespace WordreferenceBot.Scraper
{
    public class Extractor
    {
        private Request request;
        public Extractor()
        {
            request = new Request();
        }
        public async Task<IEnumerable<Word>> Extract(string word)
        {
            var wordReferencePage = await request.ResquestWord(word);
            var words = ParsePage(wordReferencePage);
            return words;
        }

        private IEnumerable<Word> ParsePage(HtmlDocument wordReferencePage)
        {
            return ExtractWordsTranslations(wordReferencePage);
        }

        private IEnumerable<Word> ExtractWordsTranslations(HtmlDocument wordReferencePage)
        {
           
            var rowsWithTranslations = wordReferencePage.DocumentNode.SelectNodes("//tr[@class='even' or @class='odd']");
            
            var words = new List<Word>();
            Word word = null;
            foreach (var row in rowsWithTranslations)
            {
                var frWord = ExtractFrWrd(row);
                if (!String.IsNullOrEmpty(frWord))
                {
                    if (word != null)
                    {
                        words.Add(word);
                    }
                    word = new Word(frWord);

                }
                var accepcion = ExtractAcception(row);
                var toWrd = ExtractToWrd(row);

                if (!String.IsNullOrEmpty(accepcion))
                {
                    word.Translations.AddMeaning(accepcion);
                }
                if (!String.IsNullOrEmpty(toWrd))
                {
                    word.Translations.AddPossibleTranslation(toWrd);
                }

            };

            return words;
        }

       private string ExtractToWrd(HtmlNode tr)
       {
            return tr.SelectNodes("td[@class='ToWrd']/node()[1]")?.First().InnerText;
       }

        private string ExtractFrWrd(HtmlNode tr)
        {
            return tr.SelectNodes("td[@class='FrWrd']/node()[1]")?.First().InnerText;
        }

        private string ExtractAcception(HtmlNode tr)
        {
            return tr.SelectNodes("td[@class='FrWrd']/following-sibling::td")?.FirstOrDefault().InnerText;
        }
        
    }
}
