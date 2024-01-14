using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;
using System.Threading;
using Pixel.Util;
using Discord;
using System;

namespace Pixel.Services;

public class InteractionHandlingService : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly InteractionService _interactions;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public InteractionHandlingService(
        DiscordSocketClient discord,
        InteractionService interactions,
        IServiceProvider services,
        ILogger<InteractionService> logger)
    {
        _interactions = interactions;
        _services = services;
        _discord = discord;
        _logger = logger;

        _interactions.Log += msg => Logger.OnLogAsync(logger, msg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _discord.Ready += () => _interactions.RegisterCommandsGloballyAsync();
        _discord.InteractionCreated += OnInteractionAsync;

        await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _interactions.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnInteractionAsync(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(_discord, interaction);
            var result = await _interactions.ExecuteCommandAsync(context, _services);

            if (interaction is SocketSlashCommand command)
                _logger.LogInformation(
                    $"Interaction used: {command.CommandName} by {command.User.Username} ({command.User.Id}) @ {context.Guild.Name} ({command.GuildId})");

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ToString());
        }
        catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
            {
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(msg => msg.Result.DeleteAsync());
            }
        }
    }
}