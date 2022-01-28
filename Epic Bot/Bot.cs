using System;
using System.Diagnostics;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using Newtonsoft.Json.Linq;
using DSharpPlus.VoiceNext;
using CustomLib;
using Epic_Bot;
using Epic_Bot.Commands;

namespace PepejdzaBot {

    public class Bot {

        public DiscordClient Client { get; private set; }
        public InteractivityExtension interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public VoiceNextExtension Voice { get; private set; }
        public static Stopwatch sw { get; private set; }

        public async Task RunAsync() {

            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration {

                Token = configJson.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug

            };

            Client = new DiscordClient(config);

            Voice = Client.UseVoiceNext(new VoiceNextConfiguration {

                EnableIncoming = true

            });

            Client.Ready += OnClientReady;

            Client.MessageCreated += Client_MessageCreated;

            Client.MessageDeleted += Client_MessageDeleted;

            Client.GuildMemberAdded += Client_GuildMemberAdded;

            Client.GuildMemberRemoved += Client_GuildMemberRemoved;

            Client.UseInteractivity(new InteractivityConfiguration {

                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.DeleteEmojis,
                Timeout = TimeSpan.FromMinutes(1)

            });

            var commandsConfig = new CommandsNextConfiguration {

                StringPrefixes = new string[] { configJson.prefix },
                UseDefaultCommandHandler = true,
                IgnoreExtraArguments = false,
                EnableMentionPrefix = true,
                EnableDefaultHelp = true,
                CaseSensitive = false,
                EnableDms = true,

            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<BasicCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);

        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e) {

            Console.WriteLine("Bot Online!");

            DiscordActivity activity = new DiscordActivity();
            DiscordClient discord = Client;
            activity.Name = "Status";
            discord.UpdateStatusAsync(activity);

            sw = new Stopwatch();

            sw.Start();

            var jobScheduler = new ActionScheduler.ActionScheduler(TimeSpan.FromSeconds(6), new Action(() => {

                UpdateStatus();

            }));

            jobScheduler.Start();

            return Task.CompletedTask;

        }

        private void UpdateStatus() {

            DiscordActivity activity = new DiscordActivity();
            DiscordClient client = Client;
            string activityName = "Active for " + sw.Elapsed.ToString().Remove(sw.Elapsed.ToString().Length - 8, 8);
            activity.Name = activityName;
            client.UpdateStatusAsync(activity);

        }

        private Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e) {

            // What happens when a message is created

            return Task.CompletedTask;

        }

        private Task Client_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e) {

            // What happens when a message is deleted

            return Task.CompletedTask;

        }

        private Task Client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e) {

            // What happens when someone joins the server

            return Task.CompletedTask;

        }

        private Task Client_GuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs e) {

            // What happens when leaves the server

            return Task.CompletedTask;

        }

    }

}
