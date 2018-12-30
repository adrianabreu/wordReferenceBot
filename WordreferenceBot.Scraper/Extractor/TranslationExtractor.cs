using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordReferenceBot.Domain;
using WordReferenceBot.Domain.Entities;

namespace WordreferenceBot.Scraper
{
    public class TranslationExtractor
    {
        private IWordReferenceRequest _request;
        public TranslationExtractor(IWordReferenceRequest request)
        {
            _request = request;
        }
        public async Task<Word> ExtractTranslation(string word)
        {
            var wordReferencePage = await _request.RequestTranslation(word);
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

            if (rowsWithTranslations == null)
            {
                return word;
            }

            Translation translation = null;
            foreach (var row in rowsWithTranslations)
            {
                var frWord = ExtractFrWrd(row)?.Trim();
                if (!String.IsNullOrEmpty(frWord))
                {
                    if (translation != null)
                    {
                        translations.Add(translation);
                    }
                    translation = new Translation(frWord);

                }
                var accepcion = ExtractAcception(row)?.Trim();
                var toWrd = ExtractToWrd(row)?.Trim();

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
