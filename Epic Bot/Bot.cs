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
using DSharpPlus.VoiceNext;
using Epic_Bot.Commands;
using Newtonsoft.Json.Linq;

namespace Epic_Bot{

    public partial class Bot {

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
            Commands.RegisterCommands<MathCommands>();
            Commands.RegisterCommands<AdminCommands>();
            Commands.RegisterCommands<VoiceCommands>();

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

        public static User GetUser(){

            var json = string.Empty;

            using (var fs = File.OpenRead("User.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = sr.ReadToEnd();

            var user = JsonConvert.DeserializeObject<User>(json);

            return user;

        }

        private Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e) {

            // What happens when a message is created

            bool hasPrefix = e.Message.Content.Contains("#");

            if (e.Guild == null || e.Guild.Id != 969296497512435833 || hasPrefix) return Task.CompletedTask;

            ExpUp(e.Author.Id, e);

            return Task.CompletedTask;

        }

        private void ExpUp(ulong id, MessageCreateEventArgs e){

            var user = GetUser();

            int experience = user.GetXp();

            var rd = new Random();

            int expPerMessage = rd.Next(15, 30);

            const int expToLevelUp = 1000;

            experience += expPerMessage;

            if (experience >= expToLevelUp){

                int leftOverExp = experience - expToLevelUp;

                LevelUp(leftOverExp, id, e);

            }

        }

        private async void LevelUp(int leftOverExp, ulong id, MessageCreateEventArgs e){

            var user = GetUser();

            int level = user.GetLevel();

            Rewrite("User.json", "xp", leftOverExp);

            Rewrite("User.json", "level", level + 1);

            var embed = new DiscordEmbedBuilder{

                Title = $"User **{e.Message.Author.Username}** just leveled up to LVL **{level + 1}**!",
                Color = DiscordColor.SpringGreen,
                Description = ":cold_face: :cold_face: :cold_face:"

            };

            await e.Channel.SendMessageAsync("<@" + e.Message.Author.Id + ">").ConfigureAwait(false);

            await e.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

        }

        public static void Rewrite(string fileName, string token, int newData){

            string jsonString = File.ReadAllText(fileName);
            JObject jObject = JsonConvert.DeserializeObject(jsonString) as JObject;
            JToken jToken = jObject.SelectToken(token);
            jToken.Replace(newData);
            string updatedJsonString = jObject.ToString();
            File.WriteAllText(fileName, updatedJsonString);

        }

        private async Task Client_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e) {

            // What happens when a message is deleted

            if (e.Guild == null) return; // Returns if the message was in a DM

            ulong[] blacklistedUsers = { 503234820457889802, 462269249902346241 };

            bool hasContent = false;
            bool hasId = false;
            ulong authorId = 0;
            string content = string.Empty;

            if (e.Message.Content != null) {

                hasContent = true;

                content = e.Message.Content;

            }

            if (e.Message.Author != null) {

                // Returns if the user is is blacklisted
                for (int i = 0; i < blacklistedUsers.Length; i++) {

                    if (e.Message.Author.Id == blacklistedUsers[i]) return;

                }

                hasId = true;

                authorId = e.Message.Author.Id;

            }

            DiscordChannel channel = e.Guild.GetDefaultChannel();

            if (e.Guild.Id == 760171471308980254) {

                channel = e.Guild.GetChannel(926892307947601960);

            } else if (e.Guild.Id == 748425033587752991) {

                channel = e.Guild.GetChannel(929070898693173349);

            }

            switch (e.Guild.Id) {

                case 760171471308980254:
                    channel = e.Guild.GetChannel(926892307947601960);
                    break;
                case 748425033587752991:
                    channel = e.Guild.GetChannel(929070898693173349);
                    break;
                case 969296497512435833:
                    channel = e.Guild.GetChannel(969296497512435836);
                    break;

            }

            if (hasId) {

                await channel.SendMessageAsync($"User **<@{authorId}>** just deleted a message in this server!").ConfigureAwait(false);

            } else {

                await channel.SendMessageAsync("Someone just deleted a message in this server!").ConfigureAwait(false);

            }

            if (hasContent) {

                await channel.SendMessageAsync($"Message: *{content}*").ConfigureAwait(false);

            }

        }

        private async void Welcome(GuildMemberAddEventArgs e) {

            DiscordChannel channel;

            switch (e.Guild.Id) {

                case 760171471308980254:
                    channel = e.Guild.GetChannel(926892307947601960);
                    await channel.SendMessageAsync($"**<@{e.Member.Id}>** just joined **{e.Guild.Name}**!\nPogChamp").ConfigureAwait(false);
                    break;
                case 748425033587752991:
                    channel = e.Guild.GetChannel(929070898693173349);
                    await channel.SendMessageAsync($"**<@{e.Member.Id}>** just joined **{e.Guild.Name}**!\nPogChamp").ConfigureAwait(false);
                    break;
                case 969296497512435833:
                    channel = e.Guild.GetChannel(969296497512435836);
                    await channel.SendMessageAsync($"**<@{e.Member.Id}>** just joined **{e.Guild.Name}**!\nPogChamp").ConfigureAwait(false);
                    break;

            }

        }

        private async void Bye(GuildMemberRemoveEventArgs e) {

            DiscordChannel channel;

            switch (e.Guild.Id) {

                case 760171471308980254:
                    channel = e.Guild.GetChannel(926892307947601960);
                    await channel.SendMessageAsync($"**<@{e.Member.Id}>** just left **{e.Guild.Name}**!\nWeirdChamp").ConfigureAwait(false); 
                    break;
                case 748425033587752991:
                    channel = e.Guild.GetChannel(929070898693173349);
                    await channel.SendMessageAsync($"**<@{e.Member.Id}>** just left **{e.Guild.Name}**!\nWeirdChamp").ConfigureAwait(false); 
                    break;
                case 969296497512435833:
                    channel = e.Guild.GetChannel(969296497512435836);
                    await channel.SendMessageAsync($"**<@{e.Member.Id}>** just left **{e.Guild.Name}**!\nWeirdChamp").ConfigureAwait(false); 
                    break;

            }

        }

        private Task Client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e) {

            // What happens when someone joins the server

            Console.WriteLine("User joined");

            if (e.Guild == null) return Task.CompletedTask; // Returns if the message was in a DM

            Welcome(e);

            return Task.CompletedTask;

        }

        private Task Client_GuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs e) {

            // What happens when someone leaves the server

            Console.WriteLine("User left");

            if (e.Guild == null) return Task.CompletedTask; // Returns if the message was in a DM

            Bye(e);

            return Task.CompletedTask;

        }

    }

}
