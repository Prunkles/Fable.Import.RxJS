module Fable.Import.RxJS.Playground.Program

open System
open Fable.Core
open Fable.Import.RxJS

open Fable.Core.JsInterop


let source1 = Rx.of' (Seq.init 100 id)

let source2 = source1 |> Rx.map (fun x -> x * 10)


let obv =
    { new IObserver<_> with
        member _.OnNext(value) = printfn $"{value}"
        member _.OnError(err) = printfn $"Error: {err}"
        member _.OnCompleted() = printfn "Completed" }

let subscription = source2 |> Rx.subscribe obv
subscription.Dispose()