open Bot
open DSharpPlus.Exceptions
open System.Threading.Tasks

[<EntryPoint>]
let main _ =
    try
        bot.ConnectAsync() |> Async.AwaitTask |> Async.RunSynchronously
        printfn "Connected successfully."
    with
        | :? UnauthorizedException -> 
            printfn "Invalid token. Aborting now."
            exit 2
    Task.Delay(-1) |> Async.AwaitTask |> Async.RunSynchronously
    0