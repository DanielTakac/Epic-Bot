using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepejdzaBot.Handlers.Dialogue.Steps{

    public interface IDialogueStep{

        Action<DiscordMessage> OnMessageAdded { get; set; }

        IDialogueStep NextStep { get; }

        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

    }

}
