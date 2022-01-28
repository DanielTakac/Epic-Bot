using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepejdzaBot.Handlers.Dialogue.Steps{

    public class IntStep : DialogueStepBase{

        private IDialogueStep _nextStep;

        private readonly int? _minValue;
        private readonly int? _maxValue;

        public IntStep(string content, IDialogueStep nextStep, int? minValue = null, int? maxValue = null) : base(content){

            _nextStep = nextStep;
            _minValue = minValue;
            _maxValue = maxValue;

        }

        public Action<int> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public void SetNextStep(IDialogueStep nextStep){

            _nextStep = nextStep;

        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user){

            var embedBuilder = new DiscordEmbedBuilder{

                Title = $"Respond below",
                Description = $"{user.Mention}, {_content}",
                Color = DiscordColor.Orange

            };

            embedBuilder.AddField("To stop the dialogue", "Use the /cancel command");

            if (_minValue.HasValue){

                embedBuilder.AddField("Min value:", $"{_minValue.Value}");

            }

            if (_maxValue.HasValue){

                embedBuilder.AddField("Max value:", $"{_maxValue.Value}");

            }

            var interactivity = client.GetInteractivity();

            while (true){

                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messageResult = await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if(messageResult.Result.Content.Equals("/cancel", StringComparison.OrdinalIgnoreCase)){

                    return true;

                }

                if(!int.TryParse(messageResult.Result.Content, out int inputValue)){

                    await TryAgain(channel, $"Your input is not an integer").ConfigureAwait(false);
                    continue;

                }

                if (_minValue.HasValue){

                    if(inputValue < _minValue.Value){

                        await TryAgain(channel, $"Your input value: {inputValue} is smaller than: {_minValue}").ConfigureAwait(false);
                        continue;

                    }

                }

                if (_maxValue.HasValue){

                    if (inputValue >_maxValue.Value){

                        await TryAgain(channel, $"Your input value: {inputValue} is larger than: {_maxValue}").ConfigureAwait(false);
                        continue;

                    }

                }

                OnValidResult(inputValue);

                return false;

            }

        }

    }

}
