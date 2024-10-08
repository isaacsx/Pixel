﻿using System.Threading.Tasks;
using Discord.Interactions;
using Discord;

namespace Pixel.Modules.Info;

public class UserCommand : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "The latency of the bot")]
    [RequireBotPermission(GuildPermission.SendMessages)]
    public Task Run() => ReplyAsync($"{Context.Client.Latency}ms");
}