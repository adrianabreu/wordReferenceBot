using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordreferenceBot.Scraper
{
    public interface IRequest
    {
        Task<HtmlDocument> ResquestWord(string word);
    }
}
