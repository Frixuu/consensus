module Commands

    open DSharpPlus.Entities
    open System.Threading.Tasks
    open Newtonsoft.Json
    open System.Net
    open System.IO
    open System
    open System.Collections.Generic

    /// <summary> Connects to a JSON endpoint
    /// and converts the response to a dictionary. </summary>
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
    
    /// <summary>
    /// Connects to a JSON endpoint, gets a specific value
    /// and responds to a Discord message with that value as an embed link.
    /// </summary>
    /// <remarks> This is usually a photo, a video or other media file. </remarks>
    let private respondWithKeyAsEmbed (message: DiscordMessage) url key =
        let query = async {
            message.Channel.TriggerTypingAsync() |> Async.AwaitTask |> ignore
            let! api = apiToDictionary url
            let builder = new DiscordEmbedBuilder()
            let embed = builder.WithImageUrl(api.[key]).Build()
            return message.RespondAsync("Here you go!", false, embed)
        }
        query |> Async.RunSynchronously :> Task

    /// <summary> Responds to a message with a random cat picture. </summary>
    let CommandRandomCat message =
        respondWithKeyAsEmbed message "https://aws.random.cat/meow" "file"
    
    /// <summary> Responds to a message with a random dog picture. </summary>
    let CommandRandomDog message =
        respondWithKeyAsEmbed message "https://dog.ceo/api/breeds/image/random" "message"

    let CommandPing (message : DiscordMessage) =
        message.RespondAsync "Pong!" :> Task

    let CommandPong (message : DiscordMessage) =
        message.RespondAsync "Ping!" :> Task
    
    /// <summary> Responds nicely to a greeting. </summary>
    let CommandHello (message : DiscordMessage) =
        message.RespondAsync(sprintf "Hello to you too, %s!" message.Author.Mention) :> Task