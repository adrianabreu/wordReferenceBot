using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using WordReferenceBot.Domain;

namespace WordreferenceBot.Scraper
{
    public class Extractor
    {
        public static IEnumerable<Word> Extract(HtmlDocument wordReferencePage)
        {
            var words = ExtractWordsTranslations(wordReferencePage);
            return words;
        }

        private static IEnumerable<Word> ExtractWordsTranslations(HtmlDocument wordReferencePage)
        {
           
            var rowsWithTranslations = wordReferencePage.DocumentNode.SelectNodes("//tr[@class='even' or @class='odd']");
            // Ahora tengo que crear una relación entre los significados y sus traducciones.
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

       private static string ExtractToWrd(HtmlNode tr)
       {
            return tr.SelectNodes("td[@class='ToWrd']/node()[1]")?.First().InnerText;
       }

        private static string ExtractFrWrd(HtmlNode tr)
        {
            return tr.SelectNodes("td[@class='FrWrd']/node()[1]")?.First().InnerText;
        }

        private static string ExtractAcception(HtmlNode tr)
        {
            return tr.SelectNodes("td[@class='FrWrd']/following-sibling::td")?.FirstOrDefault().InnerText;
        }
        
    }
}
