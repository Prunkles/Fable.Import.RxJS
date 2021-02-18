module Fable.Import.RxJS.Bi

open System
open JsTypes
open TypeTransforms


//type IBiObservable<'a> =
//    inherit IObservable<'a>
//    inherit JsObservable<'a>
//
//module BiObs =
//    
//    let ofSys (sysObs: IObservable<'a>) : IBiObservable<'a> =
//        let jsObs = Obs.sys2js sysObs
//        { new IBiObservable<'a> with
//            member _.Subscribe(sysObv) = sysObs.Subscribe(sysObv)
//            member _.subscribe(jsObv) = jsObs.subscribe(jsObv) }
//    
//    let ofJs (jsObs: JsObservable<'a>) : IBiObservable<'a> =
//        let sysObs = Obs.js2sys jsObs
//        { new IBiObservable<'a> with
//            member _.Subscribe(sysObv) = sysObs.Subscribe(sysObv)
//            member _.subscribe(jsObv) = jsObs.subscribe(jsObv) }
//    
//    let asSys (biObs: IBiObservable<'a>) : IObservable<'a> = upcast biObs
//    let asJs (biObs: IBiObservable<'a>) : JsObservable<'a> = upcast biObs
