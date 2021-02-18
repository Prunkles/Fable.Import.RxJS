namespace Fable.Import.RxJS

open System
open Fable.Core
open Fable.Core.JsInterop

open JsTypes
open TypeTransforms


[<AutoOpen>]
module private Imports =
    let rxjs: obj = importAll "rxjs"
    let ops: obj = importAll "rxjs/operators/index.js" // TODO: Import directory
    
    Disp.initPrototype rxjs
    Obs.initPrototype rxjs


module Rx =

    open Fable.Import.RxJS.Bi
    
    let of' (args: 'a seq) : IObservable<'a> =
        let args: 'a[] = Seq.toArray args
        emitJsExpr (rxjs, args) "$0.of(...$1)" //|> BiObs.ofJs |> BiObs.asSys
    
    let map (f: 'a -> 'b) (source: IObservable<'a>) : IObservable<'b> =
        emitJsExpr (ops, f, source) "$0.map($1)($2)" //|> BiObs.ofJs |> BiObs.asSys
    
    //
    // Subscribes
    //
    
    let subscribe (observer: IObserver<'a>) (source: IObservable<'a>) : IDisposable =
        source.Subscribe(observer)
    
//    let subscribeNext (onNext: 'a -> unit) (source: IObservable<'a>) : unit =
//        source?subscribe(onNext)
    
//    let subscribeCallbacks (onNext: 'a -> unit) (onError: exn -> unit) (onCompleted: unit -> unit) (source: IObservable<'a>) : unit =
//        (Obs.sys2js source)?subscribe(onNext, onError, onCompleted)