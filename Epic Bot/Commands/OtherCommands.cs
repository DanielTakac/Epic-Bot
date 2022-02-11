using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Epic_Bot.Commands {
    
    internal class OtherCommands : BaseCommandModule {

        [Command("Github")]
        public async Task GitHub(CommandContext ctx) {

            await ctx.Channel.SendMessageAsync("<@" + ctx.User.Id + ">");
            await ctx.Channel.SendMessageAsync("https://github.com/DanielTakac/Epic-Bot");

        }

    }

}
