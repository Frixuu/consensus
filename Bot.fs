module Bot 

    open DSharpPlus
    open System.IO
    open System.Threading.Tasks

    let botconf = new DiscordConfiguration()
    botconf.set_AutoReconnect true
    botconf.set_LogLevel LogLevel.Debug
    botconf.set_TokenType TokenType.Bot
    File.ReadAllText "token.txt" |> botconf.set_Token

    let bot = new DSharpPlus.DiscordClient(botconf)

    bot.add_MessageCreated (fun e -> 
        match e.Message.Content.ToLower() with
        | "!ping" -> e.Message.RespondAsync "pong!" :> Task
        | _ -> Task.FromResult null :> Task)