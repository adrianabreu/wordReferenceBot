using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordReferenceBot.Domain.Models
{
    public class WordDto
    {
        public string Value { get; set; }
        public IEnumerable<TranslationDto> Translations { get; set; }
    }
}
