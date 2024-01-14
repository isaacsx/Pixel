using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Threading;
using Pixel.Util;
using DotNetEnv;
using Discord;

namespace Pixel.Services
{
    public class BotService : IHostedService
    {
        private readonly DiscordSocketClient _discord;

        public BotService(DiscordSocketClient discord, ILogger<DiscordSocketClient> logger)
        {
            _discord = discord;

            _discord.Log += msg => Logger.OnLogAsync(logger, msg);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _discord.LoginAsync(TokenType.Bot, Env.GetString("DISCORD_TOKEN"));
            await _discord.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discord.LogoutAsync();
            await _discord.StopAsync();
        }
    }
}