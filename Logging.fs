module Logging

    open Sentry
    open System

    let dsn = Environment.GetEnvironmentVariable "SENTRY_DSN"
    let env = Environment.GetEnvironmentVariable "SENTRY_ENV"

    let private output msg =
        let now = DateTime.Now.ToString "u" 
        printfn "[%s] %s" now msg

    let capture (ex: exn) =
        sprintf "[WARN] %s" ex.Message |> output
        SentrySdk.CaptureException ex

    let info (msg: string) =
        output msg
        SentrySdk.CaptureMessage (msg, Protocol.SentryLevel.Info) |> ignore

    let logger key =
        if String.IsNullOrWhiteSpace key then
            info "No Sentry DSN provided."
        let options (o: SentryOptions) = 
            o.Dsn <- new Dsn(key)
            o.Environment <- match env with
                                 | null -> "testing"
                                 | "" -> "testing"
                                 | " " -> "testing"
                                 | _ -> env
        let handle = SentrySdk.Init options
        let id = SentrySdk.CaptureMessage ("Connected to Sentry.", Protocol.SentryLevel.Info)
        match id with
            | x when x = Sentry.Protocol.SentryId.Empty ->
                info "Connection to Sentry cannot be established. Integration disabled."
            | _ -> info "Successfully connected to Sentry."
        handle