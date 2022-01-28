# Epic-Bot

#### Dogshit discord bot made by ***Daniel Takac*** and ***Jakub Svagerko***

Made using ***C#/DSharpPlus***

#### Basic Commands:
- Ping 
```csharp
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
```

- Roll
- Random Number
- Random
- GIF
- Tag
- Time
- Channel

#### Math Commands:
- Add
- Subtract
- Multiply
- Divide
- Min
- Max
- Even Or Odd
- Sqr Root
- Rovnica

#### Admin Commands:
- Set_Status
- Custom_Message
- Exit
- Restart
- System_Info
- Ban
- Unban

#### Voice Commands:
- JoinVoice
- LeaveVoice

#### Other Commands:
- GitHub
