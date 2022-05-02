using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;

namespace Epic_Bot.Commands {

    internal class VoiceCommands : BaseCommandModule {

        [Command("JoinVoice")]
        [Aliases("vc")]
        [Description("Connect to a voice channel")]
        public async Task Join(CommandContext ctx) {

            if (ctx.Member.VoiceState == null) return;

            var vnext = ctx.Client.GetVoiceNext();

            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc != null)
                throw new InvalidOperationException("Already connected in this guild.");

            var chn = ctx.Member?.VoiceState?.Channel;
            if (chn == null)
                throw new InvalidOperationException("You need to be in a voice channel.");

            vnc = await vnext.ConnectAsync(chn);

            await ctx.RespondAsync("👌");

        }

        [Command("LeaveVoice")]
        [Description("Disconnect from a voice channel")]
        public async Task Leave(CommandContext ctx) {

            var vnext = ctx.Client.GetVoiceNext();

            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
                throw new InvalidOperationException("Not connected in this guild.");

            vnc.Disconnect();

            await ctx.RespondAsync("👌");

        }

        [Command("VoteMute")]
        [Aliases("VM")]
        [Description("Vote to mute a user")]
        public async Task VoteMute(CommandContext ctx, ulong id) {

            if (ctx.Guild == null) return; // Returns if the message was in a DM
            if (ctx.Guild.Id != 969296497512435833) return; // Returns if the message was in a different server

            ulong emoji1 = 970646703319449682;
            ulong emoji2 = 970646703382351922;

            var user = await ctx.Guild.GetMemberAsync(id);

            List<DiscordEmoji> emojiOptions = new List<DiscordEmoji>();

            emojiOptions.Add(ctx.Guild.GetEmojiAsync(emoji1).Result);
            emojiOptions.Add(ctx.Guild.GetEmojiAsync(emoji2).Result);

            var interactivity = ctx.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());

            var pollEmbed = new DiscordEmbedBuilder {

                Title = $"Mute {user.Username} for 1 minute?",
                Description = string.Join(" ", options),
                Color = DiscordColor.Orange

            };

            var pollMessage = await ctx.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            foreach (var option in emojiOptions) {

                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);

            }

            var result = await interactivity.CollectReactionsAsync(pollMessage, new TimeSpan(0, 0, 40)).ConfigureAwait(false);

            var distinctResult = result.Distinct();

            var results = distinctResult.Select(x => $"{x.Emoji}: {x.Total}");

            bool VotePassed() {

                int yepCount = 0;
                int nopeCount = 0;

                for (int i = 0; i < result.Count; i++) {

                    if (result[i].Emoji.Id == emoji1) { yepCount++; }
                    if (result[i].Emoji.Id == emoji2) { nopeCount++; }

                }

                if (yepCount >= nopeCount) {

                    return true;

                } else {

                    return false;

                }

            }

            if (VotePassed()) {

                await user.SetMuteAsync(true);

                var embed = new DiscordEmbedBuilder {

                    Title = $"{user.Username} has been muted for 1 minute!",
                    Description = ":white_check_mark:",
                    Color = DiscordColor.SpringGreen

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                await Task.Delay(new TimeSpan(0, 1, 0)).ContinueWith(o => { Unmute(user); });

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = $"{user.Username} hasn't been muted!",
                    Description = ":x:",
                    Color = DiscordColor.Red

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        public async void Unmute(DiscordMember user) {

            await user.SetMuteAsync(false);

        }

    }

}
