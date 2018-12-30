using System;
using System.Collections.Generic;

namespace WordReferenceBot.Domain.Entities
{
    public class Word
    {
        public string Value { get; set; }
        public IEnumerable<Translation> Translations { get; set; }

        public Word()
        {

        }
        public Word(string sourceWord)
        {
            Value = sourceWord;
            Translations = new List<Translation>();
        }
    }
}
