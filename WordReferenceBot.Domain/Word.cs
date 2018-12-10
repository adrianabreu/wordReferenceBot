using System;
using System.Collections.Generic;

namespace WordReferenceBot.Domain
{
    public class Word
    {
        public string Value { get; set; }
        public Translation Translations { get; set; }

        public Word(string sourceWord)
        {
            Value = sourceWord;
            Translations = new Translation();
        }
        public Word(string originalWord, Translation translations)
        {
            Value = originalWord;
            Translations = translations;
        }
    }
}
