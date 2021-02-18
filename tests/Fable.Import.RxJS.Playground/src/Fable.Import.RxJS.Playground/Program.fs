module Fable.Import.RxJS.Playground.Program

open System
open Fable.Import.RxJS


let obv =
    { new IObserver<_> with
        member _.OnNext(value) = printfn $"{value}"
        member _.OnError(err) = printfn $"Error: {err}"
        member _.OnCompleted() = printfn "Completed" }

let subscription =
    Rx.of' (Seq.init 50 id)
    |> Rx.map (fun x -> x * 10)
    |> Rx.bufferCount 10 5
    |> Rx.find (fun xs -> xs.Length = 5)
    |> Rx.subscribe obv

subscription.Dispose()