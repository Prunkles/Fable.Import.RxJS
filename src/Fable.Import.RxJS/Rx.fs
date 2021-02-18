namespace Fable.Import.RxJS

open System
open Fable.Core
open Fable.Core.JsInterop

open TypeTransforms


[<AutoOpen>]
module private Imports =
    let rxjs: obj = importAll "rxjs"
    let ops: obj = importAll "rxjs/operators/index.js" // TODO: Import directory
    
    Prototypes.Subscription ()
    Prototypes.Observable ()
    Prototypes.Subscriber ()


type IRxObservable<'a> = inherit IObservable<'a>

module RxObservable =
    
    let internal toClass (rxObs: IRxObservable<'a>) : JsTypes.Observable<'a> = !!rxObs
    let internal ofClass (jsObs: JsTypes.Observable<'a>) : IRxObservable<'a> = !!jsObs
    
    let ofObservable (source: IObservable<'a>) : IRxObservable<'a> =
        let subscribe (subscriber: JsTypes.Subscriber<'a>) : JsTypes.TeardownLogic =
            let obv = Obv.js2sys subscriber
            let disposable = source.Subscribe(obv)
            let unsubscribable = { new JsTypes.Unsubscribable with member _.unsubscribe() = disposable.Dispose() }
            JsTypes.TeardownLogic.Unsubscribable unsubscribable
        JsTypes.Observable(subscribe) |> ofClass


[<AutoOpen>]
module private Helpers =
    
    let call (operatorName: string) (args: obj seq) (source: IRxObservable<'a>) : IRxObservable<'b> =
        emitJsExpr (ops, operatorName, args, source) "$0[$1](...$2)($3)"


module Rx =
    
    let of' (args: 'a seq) : IRxObservable<'a> =
        emitJsExpr (rxjs, args) "$0.of(...$1)"
    
    let from (input: 'any) : IRxObservable<'a> =
        emitJsExpr (rxjs, input) "$0.from($1)"
    
    let merge (observables: 'any[]) : IRxObservable<'a> =
        emitJsExpr (rxjs, observables) "$0.from($1)"
    
    let map (f: 'a -> 'b) (source: IRxObservable<'a>) : IRxObservable<'b> =
        call "map" [f] source
    
    let bufferCount (bufferSize: int) (startBufferEvery: int) (source: IRxObservable<'a>) : IRxObservable<'a[]> =
        call "bufferCount" [bufferSize; startBufferEvery] source
    
    let find (predicate: 'a -> bool) (source: IRxObservable<'a>) : IRxObservable<'a option> =
        call "find" [predicate] source
    
    let flatMap (project: 'a -> #IRxObservable<'b>) (source: IRxObservable<'a>) : IRxObservable<'b> =
        call "flatMap" [project] source
    
    let window (windowBoundaries: IRxObservable<'any>) (source: IRxObservable<'a>) : IRxObservable<IRxObservable<'a>> =
        call "window" [windowBoundaries] source
    
    let single (predicate: 'a -> bool) (source: IRxObservable<'a>) : IRxObservable<'a> =
        call "single" [predicate] source
    
    let retry (source: IRxObservable<'a>) : IRxObservable<'a> =
        call "retry" [] source
    
    let retryCount (count: int) (source: IRxObservable<'a>) : IRxObservable<'a> =
        call "retry" [count] source
    
    //
    // Subscribes
    //
    
    let subscribe (observer: IObserver<'a>) (source: IRxObservable<'a>) : IDisposable =
        source.Subscribe(observer)
    
    let subscribeNext (onNext: 'a -> unit) (source: IRxObservable<'a>) : IDisposable =
        source?subscribe(onNext)
    
    let subscribeCallbacks (onNext: 'a -> unit) (onError: exn -> unit) (onCompleted: unit -> unit) (source: IObservable<'a>) : unit =
        source?subscribe(onNext, onError, onCompleted)
    