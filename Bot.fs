module Bot 

    open DSharpPlus
    open Logging
    open System
    open System.IO
    open System.Threading.Tasks
    open Commands

    let private botconf = new DiscordConfiguration()
    botconf.set_AutoReconnect true
    botconf.set_LogLevel LogLevel.Debug
    botconf.set_TokenType TokenType.Bot

    let private token = Environment.GetEnvironmentVariable "CONSENSUS_DISCORD_TOKEN"
    match token with
    | null ->
        try
            info "No token in environment variables. Searching token.txt."
            File.ReadAllText "token.txt" |> botconf.set_Token
        with
            | ex -> 
                printfn "No file named token.txt exists. Aborting now."
                capture ex |> ignore
                exit 1
    | _ -> botconf.set_Token token

    /// <summary> An instance of a Discord bot. </summary>
    let bot = new DiscordClient(botconf)

    bot.add_MessageCreated (fun e -> 
        try
            // Don't react to any messages sent by bots
            // (to prevent potential infinite loops)
            match e.Author.IsBot with
            | true -> Task.FromResult null :> Task
            | _ ->
                // Commands are case-insensitive
                match e.Message.Content.ToLower() with
                | "$cat" -> CommandRandomCat e.Message
                | "$dog" -> CommandRandomDog e.Message
                | s when s.StartsWith "!ping" -> CommandPing e.Message
                | s when s.StartsWith "!pong" -> CommandPong e.Message
                | s when List.exists (fun (g: string) -> s.Contains g) ["hi "; "hello"; "welcome"] &&
                    Seq.exists ((=)bot.CurrentUser) e.MentionedUsers -> CommandHello e.Message
                | _ -> Task.FromResult null :> Task
        with
            | ex ->
                capture ex |> ignore
                Task.FromResult null :> Task)