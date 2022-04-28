using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using PepejdzaBot;
using PepejdzaBot.Handlers.Dialogue;
using PepejdzaBot.Handlers.Dialogue.Steps;

namespace Epic_Bot.Commands {

    internal class BasicCommands : BaseCommandModule {

        [Command("Ping")]
        public async Task Ping(CommandContext ctx) {

            var embed = new DiscordEmbedBuilder {

                Title = "Pong!",
                Color = DiscordColor.Green,
                Description = ":ping_pong:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Roll")]
        [Description("Generates a random number between 1 and 6")]
        public async Task Roll(CommandContext ctx) {

            Random rd = new Random();

            int rand = rd.Next(1, 7); // Random number from 1 to 6

            await ctx.Channel.SendMessageAsync(rand.ToString()).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync("<@" + ctx.User.Id + ">").ConfigureAwait(false);

        }

        [Command("RandomNumber")]
        [Description("Generates a random number")]
        public async Task RandomNumber(CommandContext ctx, int min, int max) {

            Random rd = new Random();

            int rand = rd.Next(min, max + 1);

            var embed = new DiscordEmbedBuilder {

                Title = "Random Number\n" + rand.ToString(),
                Color = DiscordColor.Teal,
                Description = ":abacus:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Random")]
        [Description("Shows a random GIF")]
        public async Task GIF(CommandContext ctx) {

            await ctx.Channel.SendMessageAsync("Random GIF").ConfigureAwait(false);

            var giphy = new Giphy("nz4ogcR7j9ApSBnOxsc1Zj5mPXM7Jwan");

            var gifresult = await giphy.RandomGif(new RandomParameter() { });

            var result = gifresult.Data.Url;

            await ctx.Channel.SendMessageAsync(result.ToString());


        }

        [Command("GIF")]
        [Description("Searches for a GIF based on the tag parameter")]
        public async Task GIF(CommandContext ctx, [Description("Tag used to search for a GIF")][RemainingText] string tag) {

            var giphy = new Giphy("nz4ogcR7j9ApSBnOxsc1Zj5mPXM7Jwan");

            var gifresult = await giphy.RandomGif(new RandomParameter() {

                Tag = tag

            });

            var result = gifresult.Data.Url;

            if (gifresult.Data.Url == null) {

                await ctx.Channel.SendMessageAsync("Nothing found Sadge");

            }

            await ctx.Channel.SendMessageAsync("Random GIF").ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(result.ToString());

        }

        [Command("Tag")]
        [Description("Spams tags at a user")]
        public async Task Tag(CommandContext ctx, string user) {

            await ctx.Channel.SendMessageAsync(user);
            await ctx.Channel.SendMessageAsync(user);
            await ctx.Channel.SendMessageAsync(user);
            await ctx.Channel.SendMessageAsync(user);
            await ctx.Channel.SendMessageAsync(user);

        }

        [Command("Time")]
        [Description("Time since the bot started")]
        public async Task Time(CommandContext ctx) {

            var elapsed = Bot.sw.Elapsed;

            //String cutter
            string input = elapsed.ToString();
            int index = input.IndexOf(".");
            if (index >= 0) {

                input = input.Substring(0, index);

            }

            var embed = new DiscordEmbedBuilder {

                Title = $"Time since the start:\n{input}",
                Color = DiscordColor.Orange,
                Description = ":hourglass:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Channel")]
        [Description("Creates a temporary discord channel")]
        public async Task Channel(CommandContext ctx) {

            if (ctx.Guild.Id != 760171471308980254) {

                return;

            }

            var intStep3 = new IntStep("Choose how long the channel should last (in hours)", null, 1);
            var textStep2 = new TextStep("Choose a name for the temporary voice channel", intStep3);
            var textStep1 = new TextStep("Choose the type of channel to be created (voice/text)", textStep2);

            int channelTime = 0;

            string channelName = string.Empty;

            string channelType = string.Empty;

            textStep1.OnValidResult += (result1) => channelType = result1;
            textStep2.OnValidResult += (result2) => channelName = result2;
            intStep3.OnValidResult += (result3) => channelTime = result3;

            var inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, textStep1);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            DiscordChannel channel = ctx.Guild.CreateChannelAsync("Voice", ChannelType.Voice).Result;

            await channel.DeleteAsync();

            if (channelType == "voice") {

                var parent = ctx.Guild.GetChannel(767800824565858344);

                channel = ctx.Guild.CreateChannelAsync(channelName, ChannelType.Voice, parent).Result;

                channel.ModifyPositionAsync(1).Wait();

            } else if (channelType == "text") {

                var parent = ctx.Guild.GetChannel(764863805439082516);

                channel = ctx.Guild.CreateChannelAsync(channelName, ChannelType.Text, parent).Result;

                channel.ModifyPositionAsync(1).Wait();

            } else {

                var errorEmbed = new DiscordEmbedBuilder {

                    Title = "Something went wrong!",
                    Color = DiscordColor.Red,
                    Description = ":exclamation: :exclamation: :exclamation:"

                };

                var errorMessage = await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);

                return;

            }

            var embed = new DiscordEmbedBuilder {

                Title = $"Temporary {channelType} channel named '{channelName}' created and will be deleted in {channelTime} hours",
                Color = DiscordColor.Orange,
                Description = ""

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            await Task.Delay(new TimeSpan(channelTime, 0, 0)).ContinueWith(o => { DeleteChannel(channel); });

        }

        public async void DeleteChannel(DiscordChannel channel) {

            await channel.DeleteAsync("Deleting temporary channel");

        }

    }

}
