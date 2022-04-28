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

        // Finish Later

        /*[Command("VoteMute")]
        [Aliases("VM")]
        [Description("Vote to mute a user")]
        public async Task VoteMute(CommandContext ctx, ulong id) {

            if (ctx.Guild == null) return; // Returns if the message was in a DM

            var user = await ctx.Guild.GetMemberAsync(id);

            List<DiscordEmoji> emojiOptions = new List<DiscordEmoji>();

            if (ctx.Guild.Id == 760171471308980254) {

                emojiOptions.Add(ctx.Guild.GetEmojiAsync(926646675387338763).Result);
                emojiOptions.Add(ctx.Guild.GetEmojiAsync(926647059744960512).Result);

            } else if (ctx.Guild.Id == 748425033587752991) {

                emojiOptions.Add(ctx.Guild.GetEmojiAsync(929128637410721802).Result);
                emojiOptions.Add(ctx.Guild.GetEmojiAsync(929128572944269332).Result);

            }

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

                    if (result[i].Emoji.Id == 926646675387338763) { yepCount++; }
                    if (result[i].Emoji.Id == 926647059744960512) { nopeCount++; }

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

        }*/

    }

}
