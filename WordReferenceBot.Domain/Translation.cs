using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordReferenceBot.Domain
{
    public class Translation
    {
        public string WordExpression { get; set; }
        public List<string> Meanings { get; set; }
        public List<string> PossibleTranslations { get; set; }
        public Translation()
        {

        }
        public Translation(string wordExpression)
        {
            WordExpression = wordExpression;
            Meanings = new List<string>();
            PossibleTranslations = new List<string>();
        }
        public Translation(IEnumerable<string> meanings, IEnumerable<string> possibleTranslations)
        {
            Meanings = meanings.ToList();
            PossibleTranslations = possibleTranslations.ToList() ;
        }

        public void AddMeaning(string meaning)
        {
            Meanings.Add(meaning);
        }

        public void AddPossibleTranslation(string possibleTranslation)
        {
            PossibleTranslations.Add(possibleTranslation);
        }
    }
}
