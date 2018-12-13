using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WordReferenceBot.Bot.Services;

namespace WordReferenceBot.Bot.Controllers
{
    [Route("api/[controller]")]
    public class TranslateController : Controller
    {
        private readonly ITranslateService _translateService;

        public TranslateController(ITranslateService updateService)
        {
            _translateService = updateService;
        }

        // POST api/update
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            await _translateService.TranslateAsync(update);
            return Ok();
        }
    }
}
