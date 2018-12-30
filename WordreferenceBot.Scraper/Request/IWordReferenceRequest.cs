using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordreferenceBot.Scraper
{
    public interface IWordReferenceRequest
    {
        Task<HtmlDocument> RequestTranslation(string word);
    }
}
