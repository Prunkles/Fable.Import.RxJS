module Fable.Import.RxJS.JsTypes

open Fable.Core


[<Interface>]
type Observer<'T> =
    abstract next: value: 'T -> unit
    abstract error: err: exn -> unit
    abstract complete: unit -> unit
    // abstract closed: bool option


[<Interface>]
type Unsubscribable =
    abstract unsubscribe: unit -> unit


[<Interface>]
type SubscriptionLike =
    inherit Unsubscribable
    abstract closed: bool with get


[<Interface>]
type Subscribable<'T> =
    abstract subscribe: Observer<'T> -> Unsubscribable


[<Erase>]
type TeardownLogic = Unsubscribable of Unsubscribable | Function of (obj -> obj) | Void of unit


[<Class>]
[<Import("Subscription", from="rxjs")>]
type Subscription(?unsubscribe: unit -> unit) =
    
    interface SubscriptionLike with
        member this.unsubscribe() = jsNative
        member this.closed = jsNative
    
    abstract unsubscribe: unit -> unit
    default _.unsubscribe() = jsNative
    
    abstract closed: bool with get
    default _.closed = jsNative
    
    abstract add: teardown: TeardownLogic -> Subscription
    default _.add(_) = jsNative
    
    abstract remove: subscription: Subscription -> unit
    default _.remove(_) = jsNative
    
    // System
    interface System.IDisposable with
        member this.Dispose() = jsNative


[<Class>]
[<Import("Subscriber", from="rxjs")>]
type Subscriber<'T> =
    inherit Subscription
    interface Observer<'T> with
        member this.next(_) = jsNative
        member this.error(_) = jsNative
        member this.complete() = jsNative
    
    abstract next: value: 'T -> unit
    default _.next(_) = jsNative
    
    abstract error: err: exn -> unit
    default _.error(_) = jsNative
    
    abstract complete: unit -> unit
    default _.complete() = jsNative
    
    // System
    interface System.IObserver<'T> with
        member this.OnNext(_) = jsNative
        member this.OnError(_) = jsNative
        member this.OnCompleted() = jsNative


[<Interface>]
type Operator<'T, 'R> =
    abstract call: subscriber: Subscriber<'R> * source: obj -> TeardownLogic


[<Class>]
[<Import("Observable", from="rxjs")>]
type Observable<'T>(?subscribe: (Subscriber<'T> -> TeardownLogic)) =
    
    interface Subscribable<'T> with
        member this.subscribe(_) = jsNative
    
    abstract subscribe: Observer<'T> -> Subscription
    default _.subscribe(_) = jsNative
    
    abstract lift<'R> : operator: Operator<'T, 'R> -> Observable<'T>
    default _.lift(_) = jsNative
    
    abstract source: Observable<obj> with get
    default _.source = jsNative
    
    abstract operator: Operator<obj, 'T> with get
    default _.operator = jsNative
    
    // System
    interface System.IObservable<'T> with
        member this.Subscribe(_) = jsNative
