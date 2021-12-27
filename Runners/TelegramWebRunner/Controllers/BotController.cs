using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dals;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramWebRunner.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Content("Hi");

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            var value = Environment.GetEnvironmentVariable("TelegramKey");
            var client = new TelegramBotClient(value);
            var host = new Host(new MemoryDal(), new PluginsMenu(), new PluginsManager());

            if (update.Type == UpdateType.Message)
            {
                var result = host.Run(update.Message.Text, update.Message.From.Id.ToString());
                await client.SendTextMessageAsync(update.Message.From.Id, result);
            }

            return Ok();
        }
    }
}
