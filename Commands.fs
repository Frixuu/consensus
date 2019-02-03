module Commands

    open DSharpPlus.Entities
    open System.Threading.Tasks

    let CommandPing (message : DiscordMessage) =
        message.RespondAsync "Pong!" :> Task

    let CommandPong (message : DiscordMessage) =
        message.RespondAsync "Ping!" :> Task