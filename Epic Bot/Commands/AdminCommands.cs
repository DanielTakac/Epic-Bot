using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bot.Commands {
    
    internal class AdminCommands : BaseCommandModule {

        private ulong[] adminIds = { 437896136590884876, // Void()
                                     503234820457889802 }; // Nickname0081

        private bool IsAdmin(ulong id) {

            for (int i = 0; i < adminIds.Length; i++) {

                if (id == adminIds[i]) { return true; }

            }

            return false;

        }

        [Command("Set_Status")]
        [Description("Change the bot's status")]
        public async Task SetStatus(CommandContext ctx, [Description("Activity Name")] string input) {

            if (IsAdmin(ctx.User.Id)) {

                DiscordActivity activity = new DiscordActivity();
                DiscordClient discord = ctx.Client;
                activity.Name = input;
                await discord.UpdateStatusAsync(activity);

                var embed = new DiscordEmbedBuilder {

                    Title = "Status succesfully changed!",
                    Color = DiscordColor.HotPink,
                    Description = ":wrench:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                Console.WriteLine("Status changed to: " + input);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "You do not have permission to use this command.",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Custom_Message")]
        [Description("Sends a custom message to a specific channel")]
        public async Task CustomMessage(CommandContext ctx, [Description("ID of the channel where you want the message to be sent")] long channelID, [Description("Text of the message")] string message) {

            if (IsAdmin(ctx.User.Id)) {

                await ctx.Guild.GetChannel((ulong)channelID).SendMessageAsync(message);

            } else {

                await ctx.Channel.SendMessageAsync("You do not have permission to use this command.").ConfigureAwait(false);

            }

        }

        [Command("Exit")]
        [Description("Exits the application")]
        public async Task Exit(CommandContext ctx) {

            if (IsAdmin(ctx.User.Id)) {

                var embed = new DiscordEmbedBuilder {

                    Title = "Application exiting!",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                Environment.Exit(0);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "You do not have permission to use this command.",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Restart")]
        [Aliases("R")]
        [Description("Restarts the application")]
        public async Task Restart(CommandContext ctx) {

            if (IsAdmin(ctx.User.Id)) {

                var embed = new DiscordEmbedBuilder {

                    Title = "Application restarting!",
                    Color = DiscordColor.CornflowerBlue,
                    Description = ":arrows_counterclockwise:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                string path = Environment.CurrentDirectory + @"\PepejdzaBot.BotsRepaired.exe";

                Process.Start(path);

                Environment.Exit(0);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "You do not have permission to use this command.",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("System_Info")]
        [Description("Displays the system information of the pc on which the bot is running on")]
        public async Task SystemInfo(CommandContext ctx) {

            if (IsAdmin(ctx.User.Id)) {

                var os = Environment.OSVersion;
                var user = Environment.UserName;
                var cpu = Environment.ProcessorCount;
                var version = Environment.Version;
                var bit = Environment.Is64BitProcess;

                var embed = new DiscordEmbedBuilder {

                    Title = $"System Info\nOS Version: {os}\nBuild Version: {version}\nCPU Count: {cpu}\nIs64Bit: {bit}",
                    Color = DiscordColor.CornflowerBlue,
                    Description = ":desktop:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "You do not have permission to use this command.",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Ban")]
        [Description("Bans the specified user")]
        public async Task Ban(CommandContext ctx, long id) {

            if (IsAdmin(ctx.User.Id)) {

                var embed = new DiscordEmbedBuilder {

                    Title = "User " + id + " has been banned!",
                    Color = DiscordColor.Orange,
                    Description = ":orange_square:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                var member = await ctx.Guild.GetMemberAsync((ulong)id);

                await member.BanAsync();

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "You do not have permission to use this command.",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Unban")]
        [Description("Unbans the specified user")]
        public async Task Unban(CommandContext ctx, long id) {

            if (IsAdmin(ctx.User.Id)) {

                var embed = new DiscordEmbedBuilder {

                    Title = "User " + id + " has been unbanned!",
                    Color = DiscordColor.SpringGreen,
                    Description = ":green_square:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                var member = await ctx.Guild.GetMemberAsync((ulong)id);

                await member.UnbanAsync();

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "You do not have permission to use this command.",
                    Color = DiscordColor.Red,
                    Description = ":no_entry_sign:"

                };

                var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

    }

}
