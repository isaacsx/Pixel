using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Discord.Interactions;
using Discord.WebSocket;
using Pixel.Services;
using DotNetEnv;

namespace Pixel;

internal abstract class Program
{
    public static async Task Main(string[] args)
    {
        Env.TraversePath().Load();

        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<DiscordSocketClient>();
                services.AddSingleton<InteractionService>();
                    
                services.AddHostedService<InteractionHandlingService>();
                services.AddHostedService<BotService>();
            })
            .Build();

        await host.RunAsync();
    }
}