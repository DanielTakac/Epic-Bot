using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PepejdzaBot.Handlers.Dialogue;
using PepejdzaBot.Handlers.Dialogue.Steps;

namespace Epic_Bot.Commands {

    internal class MathCommands : BaseCommandModule {

        [Command("Math.Add")]
        [Description("Adds two numbers together")]
        public async Task Add(CommandContext ctx, double NumberOne, double NumberTwo) {

            var embed = new DiscordEmbedBuilder {

                Title = "Math.Add\n" + NumberOne + " + " + NumberTwo + " = " + (NumberOne + NumberTwo).ToString(),
                Color = DiscordColor.Teal,
                Description = ":abacus:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Math.Subtract")]
        [Description("Subtracts the second number from the first number")]
        public async Task Subtract(CommandContext ctx, double NumberOne, double NumberTwo) {

            var embed = new DiscordEmbedBuilder {

                Title = "Math.Subtract\n" + NumberOne + " - " + NumberTwo + " = " + (NumberOne - NumberTwo).ToString(),
                Color = DiscordColor.Teal,
                Description = ":abacus:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Math.Multiply")]
        [Description("Multilpies two numbers")]
        public async Task Multiply(CommandContext ctx, double NumberOne, double NumberTwo) {

            var embed = new DiscordEmbedBuilder {

                Title = "Math.Multiply\n" + NumberOne + " * " + NumberTwo + " = " + (NumberOne * NumberTwo).ToString(),
                Color = DiscordColor.Teal,
                Description = ":abacus:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Math.Divide")]
        [Description("Adds two numbers together")]
        public async Task Divide(CommandContext ctx, double NumberOne, double NumberTwo) {

            var embed = new DiscordEmbedBuilder {

                Title = "Math.Divide\n" + NumberOne + " / " + NumberTwo + " = " + (NumberOne / NumberTwo).ToString(),
                Color = DiscordColor.Teal,
                Description = ":abacus:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Math.Min")]
        [Description("Finds the smallest number")]
        public async Task Min(CommandContext ctx, double NumberOne, double NumberTwo) {

            if (NumberOne < NumberTwo) {

                var embed = new DiscordEmbedBuilder {

                    Title = "Math.Min\n" + NumberOne + " < " + NumberTwo,
                    Color = DiscordColor.Teal,
                    Description = ":abacus:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "Math.Min\n" + NumberTwo + " < " + NumberOne,
                    Color = DiscordColor.Teal,
                    Description = ":abacus:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Math.Max")]
        [Description("Finds the biggest number")]
        public async Task Max(CommandContext ctx, double NumberOne, double NumberTwo) {

            if (NumberOne > NumberTwo) {

                var embed = new DiscordEmbedBuilder {

                    Title = "Math.Max\n" + NumberOne + " > " + NumberTwo,
                    Color = DiscordColor.Teal,
                    Description = ":abacus:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "Math.Max\n" + NumberTwo + " > " + NumberOne,
                    Color = DiscordColor.Teal,
                    Description = ":abacus:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Math.EvenOrOdd")]
        [Description("Checks if the number is even or odd")]
        public async Task isEven(CommandContext ctx, double number) {

            if ((number % 2) == 0) {

                var embed = new DiscordEmbedBuilder {

                    Title = "Math.EvenOrOdd\n" + number + " is even",
                    Color = DiscordColor.Teal,
                    Description = ":abacus:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            } else {

                var embed = new DiscordEmbedBuilder {

                    Title = "Math.EvenOrOdd\n" + number + " is odd",
                    Color = DiscordColor.Teal,
                    Description = ":abacus:"

                };

                await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            }

        }

        [Command("Math.SqrRoot")]
        [Description("Finds the number's square root")]
        public async Task sqrRoot(CommandContext ctx, double number) {

            var embed = new DiscordEmbedBuilder {

                Title = "Math.SqrRoot\n" + "Square root of " + number + " is " + (Math.Sqrt(number)).ToString(),
                Color = DiscordColor.Teal,
                Description = ":abacus:"

            };

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        [Command("Rovnica")]
        [Description("Vypocita kvadraticku rovnicu")]
        public async Task Dialogue(CommandContext ctx) {

            int a = 0;
            int b = 0;
            int c = 0;

            var inputC = new IntStep("Input c", null);
            var inputB = new IntStep("Input b", inputC);
            var inputA = new IntStep("Input a", inputB);

            inputA.OnValidResult += (result) => a = result;
            inputB.OnValidResult += (result) => b = result;
            inputC.OnValidResult += (result) => c = result;

            var inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, inputA);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) return;

            string V = string.Empty;

            int D = b * b - (4 * a * c);

            double Dsqrt = Math.Sqrt(D);

            int answer1 = (-b + (int)Dsqrt) / (2 * a);
            int answer2 = (-b - (int)Dsqrt) / (2 * a);

            var embed1 = new DiscordEmbedBuilder {

                Title = $"{a}x^ + {b}x + {c} = 0\n" +
                        $"a = {a} ---> a > 0 ---> Vmin\n" +
                        $"--------------------------------\n" +
                        $"D = b^ - 4ac\n" +
                        $"D = {b} * {b} - 4 * {a} * {c}\n" +
                        $"D = {D}\n" +
                        $"--------------------------------\n" +
                        $"x1 = (-b + Dsqrt) / 2a\n" +
                        $"x2 = (-b - Dsqrt) / 2a\n" +
                        $"--------------------------------\n" +
                        $"x1 = {answer1}\n" +
                        $"x2 = {answer2}",
                Color = DiscordColor.Orange,
                Description = ":abacus:"

            };

            var embed2 = new DiscordEmbedBuilder {

                Title = $"{a}x^ + {b}x + {c} = 0\n" +
                        $"a = {a} ---> a < 0 ---> Vmax\n" +
                        $"--------------------------------\n" +
                        $"D = b^ - 4ac\n" +
                        $"D = {b} * {b} - 4 * {a} * {c}\n" +
                        $"D = {D}\n" +
                        $"--------------------------------\n" +
                        $"x1 = (-b + Dsqrt) / 2a\n" +
                        $"x2 = (-b - Dsqrt) / 2a\n" +
                        $"--------------------------------\n" +
                        $"x1 = {answer1}\n" +
                        $"x2 = {answer2}",
                Color = DiscordColor.Orange,
                Description = ":abacus:"

            };

            var embed = embed1;

            if (a > 0) {

                embed = embed1;

                V = "min";

            } else {

                embed = embed2;

                V = "max";

            }

            await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

    }

}
