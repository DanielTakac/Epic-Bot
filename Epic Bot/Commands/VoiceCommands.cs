using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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

    }

}
