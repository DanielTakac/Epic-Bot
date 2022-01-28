using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bot.Commands {

    internal class BasicCommands : BaseCommandModule {

        [Command("Ping")]
        [Description("")]
        public async Task Ping(CommandContext ctx) {

            var embed = new DiscordEmbedBuilder {

                Title = "Pong!",
                Color = DiscordColor.Green,
                Description = ":ping_pong:"

            };

            var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Roll")]
        [Description("Generates a random number between 1 and 6")]
        public async Task Roll(CommandContext ctx) {

            Random rd = new Random();

            int rand = rd.Next(1, 7);

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

            var message = await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

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
        public async Task GIF(CommandContext ctx, [Description("Tag used to search for a GIF")] string tag) {

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

    }

}
