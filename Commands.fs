module Commands

    open DSharpPlus.Entities
    open System.Threading.Tasks
    open Newtonsoft.Json
    open System.Net
    open System.IO
    open System
    open System.Collections.Generic

    let private apiToDictionary url =
        async {
            let uri = Uri url
            let req = WebRequest.Create uri
            use! resp = req.AsyncGetResponse()
            use stream = resp.GetResponseStream()
            use reader = new StreamReader(stream)
            let! content = reader.ReadToEndAsync() |> Async.AwaitTask
            return JsonConvert.DeserializeObject<Dictionary<string, string>> content
        }

    let private respondWithKeyAsEmbed (message: DiscordMessage) url key =
        let query = async {
            message.Channel.TriggerTypingAsync() |> Async.AwaitTask |> ignore
            let! api = apiToDictionary url
            let builder = new DiscordEmbedBuilder()
            let embed = builder.WithImageUrl(api.[key]).Build()
            return message.RespondAsync("Here you go!", false, embed)
        }
        query |> Async.RunSynchronously :> Task

    let CommandRandomCat message =
        respondWithKeyAsEmbed message "https://aws.random.cat/meow" "file"

    let CommandRandomDog message =
        respondWithKeyAsEmbed message "https://dog.ceo/api/breeds/image/random" "message"

    let CommandPing (message : DiscordMessage) =
        message.RespondAsync "Pong!" :> Task

    let CommandPong (message : DiscordMessage) =
        message.RespondAsync "Ping!" :> Task

    let CommandHello (message : DiscordMessage) =
        message.RespondAsync(sprintf "Hello to you too, %s!" message.Author.Mention) :> Task