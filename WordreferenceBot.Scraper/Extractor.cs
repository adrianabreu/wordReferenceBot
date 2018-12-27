using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordReferenceBot.Domain;

namespace WordreferenceBot.Scraper
{
    public class Extractor
    {
        private IRequest _request;
        public Extractor(IRequest request)
        {
            _request = request;
        }
        public async Task<Word> Extract(string word)
        {
            var wordReferencePage = await _request.ResquestWord(word);
            var words = ParsePage(word, wordReferencePage);
            return words;
        }

        private Word ParsePage(string sourceWord, HtmlDocument wordReferencePage)
        {
            return ExtractWordsTranslations(sourceWord,wordReferencePage);
        }

        private Word ExtractWordsTranslations(string sourceWord, HtmlDocument wordReferencePage)
        {
            var translations = new List<Translation>();
            var rowsWithTranslations = wordReferencePage.DocumentNode.SelectNodes("//tr[@class='even' or @class='odd']");

            Word word = new Word(sourceWord);
            Translation translation = null;
            foreach (var row in rowsWithTranslations)
            {
                var frWord = ExtractFrWrd(row);
                if (!String.IsNullOrEmpty(frWord))
                {
                    if (translation != null)
                    {
                        translations.Add(translation);
                    }
                    translation = new Translation(frWord);

                }
                var accepcion = ExtractAcception(row);
                var toWrd = ExtractToWrd(row);

                if (!String.IsNullOrEmpty(accepcion))
                {
                    accepcion = Regex.Replace(accepcion, @"&nbsp;", " ").Trim();
                    translation.AddMeaning(accepcion);
                }
                if (!String.IsNullOrEmpty(toWrd))
                {
                    translation.AddPossibleTranslation(toWrd);
                }

            };
            word.Translations = translations;
            return word;
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
