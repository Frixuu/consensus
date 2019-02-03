module Bot 

    open DSharpPlus
    open System
    open System.IO
    open System.Threading.Tasks
    open Commands

    let botconf = new DiscordConfiguration()
    botconf.set_AutoReconnect true
    botconf.set_LogLevel LogLevel.Debug
    botconf.set_TokenType TokenType.Bot

    let token = Environment.GetEnvironmentVariable "CONSENSUS_DISCORD_TOKEN"
    match token with
    | null ->
        try
            printfn "No token in environment variables. Searching token.txt."
            File.ReadAllText "token.txt" |> botconf.set_Token
        with
            | :? FileNotFoundException -> 
                printfn "No file named token.txt exists. Aborting now."
                exit 1
    | _ -> botconf.set_Token token

    let bot = new DiscordClient(botconf)

    bot.add_MessageCreated (fun e -> 
        match e.Message.Content.ToLower() with
        | s when s.StartsWith("!ping") -> CommandPing e.Message
        | s when s.StartsWith("!pong") -> CommandPong e.Message
        | s when s.StartsWith("!hi") -> CommandHello e.Message
        | _ -> Task.FromResult null :> Task)