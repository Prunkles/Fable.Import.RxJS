module Fable.Import.RxJS.TypeTransforms

open System

open Fable.Core


module Obv =

    let js2sys (jsObv: JsTypes.Observer<'a>) : IObserver<'a> =
        { new IObserver<_> with
            member _.OnNext(x) = jsObv.next(x)
            member _.OnError(err) = jsObv.error(err)
            member _.OnCompleted() = jsObv.complete()
        }
    
    let sys2js (obv: IObserver<'a>) : JsTypes.Observer<'a> =
        { new JsTypes.Observer<'a> with
            member _.next(x) = obv.OnNext(x)
            member _.error(err) = obv.OnError(err)
            member _.complete() = obv.OnCompleted()
        }

module Disp =
    
    open Fable.Core.JsInterop
    
    let initPrototype (rxjs: obj) =
//        rxjs?Subscription?prototype?Dispose <-
//            fun () ->
//                let this: JsTypes.Subscription = jsThis
//                this.unsubscribe()
        emitJsStatement (rxjs) """
            const $rxjs = $0;
            rxjs.Subscription.prototype.Dispose = function() {
                this.unsubscribe();
            };
        """
    
    let js2sys (jsDisp: JsTypes.Subscription) : IDisposable =
        !!jsDisp
//    let sys2js (disp: IDisposable) : JsDisposable =
//        { new JsDisposable with member _.unsubscribe() = disp.Dispose() }

module Obs =
    
    open Fable.Core.JsInterop
    
    let initPrototype (rxjs: obj) =
//        let subscribe (sysObv: IObserver<_>) : IDisposable =
//            let this: JsTypes.Observable<_> = jsThis
//            JS.console.log(this)
//            let jsObv = Obv.sys2js sysObv
//            let subscription = this.subscribe(jsObv)
//            Disp.js2sys subscription
//        rxjs?Observable?prototype?Subscribe <- subscribe
        emitJsStatement (rxjs, Obv.sys2js, Disp.js2sys) """
            const $rxjs = $0;
            const $Obv_sys2js = $1;
            const $Disp_js2sys = $2;
            $rxjs.Observable.prototype.Subscribe = function(sysObv) {
                const jsObv = $Obv_sys2js(sysObv);
                const subscription = this.subscribe(jsObv);
                return $Disp_js2sys(subscription);
            };
        """
    
    let js2sys (jsObs: JsTypes.Observable<'a>) : IObservable<'a> =
        !!jsObs
    
//    let sys2js (obs: IObservable<'a>) : JsObservable<'a> =
//        { new JsObservable<'a> with
//            member _.subscribe(jsObv) =
//                obs.Subscribe(Obv.js2sys jsObv) |> Disp.sys2js
//        }
