open Bot
open System.Threading.Tasks

[<EntryPoint>]
let main _ =
    bot.ConnectAsync() |> Async.AwaitTask |> Async.RunSynchronously
    Task.Delay(-1) |> Async.AwaitTask |> Async.RunSynchronously
    0