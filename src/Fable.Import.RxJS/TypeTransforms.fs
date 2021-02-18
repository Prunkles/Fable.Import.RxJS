module internal Fable.Import.RxJS.TypeTransforms

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


module Prototypes =
    
    open Fable.Core.JsInterop
    
    let private rxjs: obj = importAll "rxjs"
    
    let Subscription () : unit =
        emitJsStatement (rxjs) """
            // console.debug("RxJS.Subscription.prototype")
            const $rxjs = $0;
            rxjs.Subscription.prototype.Dispose = function() {
                this.unsubscribe();
            };
        """

    let Observable () : unit =
        emitJsStatement (rxjs, Obv.sys2js) """
            // console.debug("RxJS.Observable.prototype")
            const $rxjs = $0;
            const $Obv_sys2js = $1;
            $rxjs.Observable.prototype.Subscribe = function(sysObv) {
                const jsObv = $Obv_sys2js(sysObv);
                const subscription = this.subscribe(jsObv);
                return subscription;
            };
        """
    
    let Subscriber () : unit =
        emitJsStatement (rxjs) """
            const $rxjs = $0;
            $rxjs.Subscriber.prototype.OnNext = function(value) {
                this.next(value);
            };
            $rxjs.Subscriber.prototype.OnError = function(err) {
                this.error(err);
            };
            $rxjs.Subscriber.prototype.OnCompleted = function() {
                this.complete();
            };
        """
