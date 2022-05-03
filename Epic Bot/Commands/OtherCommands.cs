using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json.Linq;

namespace Epic_Bot.Commands{

    internal class OtherCommands : BaseCommandModule{

        [Command("Balance")]
        [Aliases("BAL")]
        [Description("Displays the users balance")]
        public async Task Balance(CommandContext ctx){

            var user = Bot.GetUser();

            var embed = new DiscordEmbedBuilder{

                Title = $"Your balance is **${user.GetBalance()}**",
                Color = DiscordColor.Orange,
                Description = ":moneybag:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Roulette")]
        [Description("")]
        public async Task Roulette(CommandContext ctx, string color, string bet){

            var user = Bot.GetUser();

            var errorEmbed = new DiscordEmbedBuilder {

                Title = "Wrong command format!\nCorrect format: '#roulette (string color) (int betAmount)'\nColor: red, black or green\nbetAmount: More than 1 or 'max'",
                Color = DiscordColor.Red,
                Description = ":exclamation: :exclamation: :exclamation:"

            };

            var tooBigEmbed = new DiscordEmbedBuilder {

                Title = "You entered a bet larger than your balance!",
                Color = DiscordColor.Red,
                Description = ":exclamation: :exclamation: :exclamation:"

            };

            int betAmount = 0;

            if (bet == "max"){

                betAmount = user.GetBalance();

            } else {

                betAmount = Convert.ToInt32(bet);

            }

            if (betAmount < 1) {
                
                await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                return;

            }

            if (betAmount > user.GetBalance()) {

                var tooBigMessage = await ctx.Channel.SendMessageAsync(embed: tooBigEmbed).ConfigureAwait(false);
                return;

            }

            string winEmoji = ":gem:";

            string loseEmoji = ":cold_face:";

            if (betAmount < 1000) winEmoji = ":dollar:";
            if (betAmount < 10000 && betAmount >= 1000) winEmoji = ":euro:";
            if (betAmount < 100000 && betAmount >= 10000) winEmoji = ":pound:";
            if (betAmount < 1000000 && betAmount >= 100000) winEmoji = ":yen:";

            var rd = new Random();

            int rand = rd.Next(1, 38);

            switch (color) {

                case "red":

                    if (rand <= 18) {

                        //Won
                        int balance = user.GetBalance();

                        balance += betAmount;

                        var embed = new DiscordEmbedBuilder {

                            Title = $"You won **${betAmount}**\nNew balance: **${balance}**",
                            Color = DiscordColor.SpringGreen,
                            Description = winEmoji

                        };

                        await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                        Bot.Rewrite("User.json", "balance", balance);

                    } else {

                        //Lose
                        int balance = user.GetBalance();

                        balance -= betAmount;

                        var embed = new DiscordEmbedBuilder {

                            Title = $"You lost **${betAmount}**\nNew balance: **${balance}**",
                            Color = DiscordColor.IndianRed,
                            Description = loseEmoji

                        };

                        await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                        Bot.Rewrite("User.json", "balance", balance);

                    }

                    break;

                case "black":

                    if (rand <= 18) {

                        //Won
                        int balance = user.GetBalance();

                        balance += betAmount;

                        var embed = new DiscordEmbedBuilder {

                            Title = $"You won **${betAmount}**\nNew balance: **${balance}**",
                            Color = DiscordColor.SpringGreen,
                            Description = winEmoji

                        };

                        await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                        Bot.Rewrite("User.json", "balance", balance);

                    } else {

                        //Lose
                        int balance = user.GetBalance();

                        balance -= betAmount;

                        var embed = new DiscordEmbedBuilder{

                            Title = $"You lost **${betAmount}**\nNew balance: **${balance}**",
                            Color = DiscordColor.IndianRed,
                            Description = loseEmoji

                        };

                        await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                        Bot.Rewrite("User.json", "balance", balance);

                    }

                    break;
                case "green":

                    if (rand <= 1) {

                        //Won
                        int balance = user.GetBalance();

                        balance += betAmount * 36;

                        var embed = new DiscordEmbedBuilder {

                            Title = $"You won **${betAmount * 36}**\nGreen Pog Pog!\nNew balance: **${balance}**",
                            Color = DiscordColor.SpringGreen,
                            Description = ":gem: :gem: :gem:"

                        };

                        await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                        Bot.Rewrite("User.json", "balance", balance);

                    } else {

                        //Lose
                        int balance = user.GetBalance();

                        balance -= betAmount;

                        var embed = new DiscordEmbedBuilder{

                            Title = $"You lost **${betAmount}**\nNew balance: **${balance}**",
                            Color = DiscordColor.IndianRed,
                            Description = loseEmoji

                        };

                        await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

                        Bot.Rewrite("User.json", "balance", balance);

                    }

                    break;

                default:
                    await ctx.Channel.SendMessageAsync(embed: errorEmbed).ConfigureAwait(false);
                    break;

            }

        }

        [Command("Search")]
        [Description("Gives you some money if you have less than $5000")]
        public async Task Roulette(CommandContext ctx) {

            var user = Bot.GetUser();

            if (user.GetBalance() <= 5000) {

                var rd = new Random();

                int rand = rd.Next(100, 3000); // Generates a random number between 100 and 3000

                int balance = user.GetBalance() + rand;

                Bot.Rewrite("User.json", "balance", balance);

                var embed1 = new DiscordEmbedBuilder {

                    Title = $"**OMG!** You found **${rand}** on the ground!",
                    Color = DiscordColor.HotPink,
                    Description = ":scream: :scream: :scream:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed1).ConfigureAwait(false);

            } else {

                var embed2 = new DiscordEmbedBuilder {

                    Title = "You need to have less than **$5000** to use this command!",
                    Color = DiscordColor.Red,
                    Description = ":exclamation: :exclamation: :exclamation:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed2).ConfigureAwait(false);

            }

        }

        [Command("Level")]
        [Aliases("LVL")]
        [Description("Displays your level")]
        public async Task Level(CommandContext ctx) {

            int level = Bot.GetUser().GetLevel();

            int experience = Bot.GetUser().GetXp();

            string progress = string.Empty;

            if (experience >= 0 && experience < 100) progress = "[- - - - - - - - - -]";
            if (experience >= 100 && experience < 200) progress = "[# - - - - - - - - -]";
            if (experience >= 200 && experience < 300) progress = "[# # - - - - - - - -]";
            if (experience >= 300 && experience < 400) progress = "[# # # - - - - - - -]";
            if (experience >= 400 && experience < 500) progress = "[# # # # - - - - - -]";
            if (experience >= 500 && experience < 600) progress = "[# # # # # - - - - -]";
            if (experience >= 600 && experience < 700) progress = "[# # # # # # - - - -]";
            if (experience >= 700 && experience < 800) progress = "[# # # # # # # - - -]";
            if (experience >= 800 && experience < 900) progress = "[# # # # # # # # - -]";
            if (experience >= 900 && experience < 1000) progress = "[# # # # # # # # # -]";

            var embed = new DiscordEmbedBuilder{

                Title = $"Level - {level}\nXP - {experience} / 1000\n{progress}",
                Color = DiscordColor.Cyan,
                Description = ":regional_indicator_l: :regional_indicator_e: :regional_indicator_v: :regional_indicator_e: :regional_indicator_l:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

    }

}
