using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordReferenceBot.Api.Models
{
    public class TranslationDto
    {
        public string WordExpression { get; set; }
        public List<string> Meanings { get; set; }
        public List<string> PossibleTranslations { get; set; }
        public TranslationDto()
        {

        }
    }
}
