open Bot
open Logging
open System.Threading.Tasks

[<EntryPoint>]
let main _ =
    
    use sentry = logger
    try
        bot.ConnectAsync() |> Async.AwaitTask |> Async.RunSynchronously
        log "Connected successfully to Discord servers."
    with
        | ex -> 
            printfn "Invalid token. Aborting now."
            capture ex |> ignore
            exit 2
    Task.Delay(-1) |> Async.AwaitTask |> Async.RunSynchronously
    0