module Commands

    open DSharpPlus.Entities
    open System.Threading.Tasks

    let CommandPing (message : DiscordMessage) =
        message.RespondAsync "Pong!" :> Task