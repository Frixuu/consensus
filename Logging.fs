module Logging

    open Sentry
    open System

    let dsn = Environment.GetEnvironmentVariable "SENTRY_DSN"

    let logger =
        SentrySdk.Init dsn

    let capture ex =
        SentrySdk.CaptureException ex

    let log msg =
        printfn "%s" msg
        SentrySdk.CaptureMessage (msg, Protocol.SentryLevel.Info) |> ignore