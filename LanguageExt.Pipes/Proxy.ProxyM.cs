using System;
using System.Diagnostics.Contracts;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace LanguageExt.Pipes;

/// <summary>
/// One of the algebraic cases of the `Proxy` type.  This type lifts a `Transducer` computation into the
/// `Proxy` monad-transformer.  This is how the `Proxy` system can cause real-world effects.
/// </summary>
/// <typeparam name="UOut">Upstream out type</typeparam>
/// <typeparam name="UIn">Upstream in type</typeparam>
/// <typeparam name="DIn">Downstream in type</typeparam>
/// <typeparam name="DOut">Downstream uut type</typeparam>
/// <typeparam name="A">The monadic bound variable - it doesn't flow up or down stream, it works just like any bound
/// monadic variable.  If the effect represented by the `Proxy` ends, then this will be the result value.
///
/// When composing `Proxy` sub-types (like `Producer`, `Pipe`, `Consumer`, etc.)  </typeparam>
internal record ProxyM<UOut, UIn, DIn, DOut, M, A>(K<M, Proxy<UOut, UIn, DIn, DOut, M, A>> Value) : 
    Proxy<UOut, UIn, DIn, DOut, M, A>
    where M : Monad<M>
{
    /// <summary>
    /// When working with sub-types, like `Producer`, calling this will effectively cast the sub-type to the base.
    /// </summary>
    /// <returns>A general `Proxy` type from a more specialised type</returns>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, A> ToProxy() => 
        this;

    /// <summary>
    /// Monadic bind operation, for chaining `Proxy` computations together
    /// </summary>
    /// <param name="f">The bind function</param>
    /// <typeparam name="B">The mapped bound value type</typeparam>
    /// <returns>A new `Proxy` that represents the composition of this `Proxy` and the result of the bind operation</returns>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, B> Bind<B>(Func<A, Proxy<UOut, UIn, DIn, DOut, M, B>> f) =>
        new ProxyM<UOut, UIn, DIn, DOut, M, B>(M.Map(p => p.Bind(f), Value));

    /// <summary>
    /// Lifts a pure function into the `Proxy` domain, causing it to map the bound value within
    /// </summary>
    /// <param name="f">The map function</param>
    /// <typeparam name="B">The mapped bound value type</typeparam>
    /// <returns>A new `Proxy` that represents the composition of this `Proxy` and the result of the map operation</returns>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, B> Map<B>(Func<A, B> f) =>
        new ProxyM<UOut, UIn, DIn, DOut, M, B>(M.Map(p => p.Map(f), Value));

    /// <summary>
    /// Map the lifted monad
    /// </summary>
    /// <param name="f">The map function</param>
    /// <typeparam name="B">The mapped bound value type</typeparam>
    /// <returns>A new `Proxy` that represents the composition of this `Proxy` and the result of the map operation</returns>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, B> MapM<B>(Func<K<M, A>, K<M, B>> f) =>
        new ProxyM<UOut, UIn, DIn, DOut, M, B>(M.Map(p => p.MapM(f), Value));
    
    /// <summary>
    /// Extract the lifted IO monad (if there is one)
    /// </summary>
    /// <param name="f">The map function</param>
    /// <returns>A new `Proxy` that represents the innermost IO monad, if it exists.</returns>
    /// <exception cref="ExceptionalException">`Errors.UnliftIONotSupported` if there's no IO monad in the stack</exception>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, IO<A>> ToIO() =>
        new ProxyM<UOut, UIn, DIn, DOut, M, IO<A>>(M.Map(p => p.ToIO(), Value));

    /// <summary>
    /// `For(body)` loops over the `Proxy p` replacing each `yield` with `body`
    /// </summary>
    /// <param name="body">Any `yield` found in the `Proxy` will be replaced with this function.  It will be composed so
    /// that the value yielded will be passed to the argument of the function.  That returns a `Proxy` to continue the
    /// processing of the computation</param>
    /// <returns></returns>
    [Pure]
    public override Proxy<UOut, UIn, C1, C, M, A> For<C1, C>(Func<DOut, Proxy<UOut, UIn, C1, C, M, DIn>> body) =>
        ReplaceRespond(body);

    /// <summary>
    /// Applicative action
    ///
    /// Invokes this `Proxy`, then the `Proxy r` 
    /// </summary>
    /// <param name="r">`Proxy` to run after this one</param>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, S> Action<S>(Proxy<UOut, UIn, DIn, DOut, M, S> r) =>
        new ProxyM<UOut, UIn, DIn, DOut, M, S>(M.Map(p => p.Action(r), Value));

    /// <summary>
    /// Used by the various composition functions and when composing proxies with the `|` operator.  You usually
    /// wouldn't need to call this directly, instead either pipe them using `|` or call `Proxy.compose(lhs, rhs)` 
    /// </summary>
    /// <remarks>
    /// (f +>> p) pairs each 'request' in `this` with a 'respond' in `fb1`.
    /// </remarks>
    [Pure]
    public override Proxy<UOutA, AUInA, DIn, DOut, M, A> PairEachRequestWithRespond<UOutA, AUInA>(
        Func<UOut, Proxy<UOutA, AUInA, UOut, UIn, M, A>> fb1) =>
        new ProxyM<UOutA, AUInA, DIn, DOut, M, A>(M.Map(p1 => p1.PairEachRequestWithRespond(fb1), Value));

    /// <summary>
    /// Used by the various composition functions and when composing proxies with the `|` operator.  You usually
    /// wouldn't need to call this directly, instead either pipe them using `|` or call `Proxy.compose(lhs, rhs)` 
    /// </summary>
    [Pure]
    public override Proxy<UOutA, AUInA, DIn, DOut, M, A> ReplaceRequest<UOutA, AUInA>(
        Func<UOut, Proxy<UOutA, AUInA, DIn, DOut, M, UIn>> lhs) =>
        new ProxyM<UOutA, AUInA, DIn, DOut, M, A>(M.Map(p => p.ReplaceRequest(lhs), Value));

    /// <summary>
    /// Used by the various composition functions and when composing proxies with the `|` operator.  You usually
    /// wouldn't need to call this directly, instead either pipe them using `|` or call `Proxy.compose(lhs, rhs)` 
    /// </summary>
    [Pure]
    public override Proxy<UOut, UIn, DInC, DOutC, M, A> PairEachRespondWithRequest<DInC, DOutC>(
        Func<DOut, Proxy<DIn, DOut, DInC, DOutC, M, A>> rhs) =>
        new ProxyM<UOut, UIn, DInC, DOutC, M, A>(M.Map(p1 => p1.PairEachRespondWithRequest(rhs), Value));

    /// <summary>
    /// Used by the various composition functions and when composing proxies with the `|` operator.  You usually
    /// wouldn't need to call this directly, instead either pipe them using `|` or call `Proxy.compose(lhs, rhs)` 
    /// </summary>
    [Pure]
    public override Proxy<UOut, UIn, DInC, DOutC, M, A> ReplaceRespond<DInC, DOutC>(
        Func<DOut, Proxy<UOut, UIn, DInC, DOutC, M, DIn>> rhs) =>
        new ProxyM<UOut, UIn, DInC, DOutC, M, A>(M.Map(p => p.ReplaceRespond(rhs), Value));

    /// <summary>
    /// Reverse the arrows of the `Proxy` to find its dual.  
    /// </summary>
    /// <returns>The dual of `this1</returns>
    [Pure]
    public override Proxy<DOut, DIn, UIn, UOut, M, A> Reflect() =>
        new ProxyM<DOut, DIn, UIn, UOut, M, A>(M.Map(p => p.Reflect(), Value));

    /// <summary>
    /// 
    ///     Observe(lift (Pure(r))) = Observe(Pure(r))
    ///     Observe(lift (m.Bind(f))) = Observe(lift(m.Bind(x => lift(f(x)))))
    /// 
    /// This correctness comes at a small cost to performance, so use this function sparingly.
    /// This function is a convenience for low-level pipes implementers.  You do not need to
    /// use observe if you stick to the safe API.        
    /// </summary>
    [Pure]
    public override Proxy<UOut, UIn, DIn, DOut, M, A> Observe() =>
        new ProxyM<UOut, UIn, DIn, DOut, M, A>(M.Map(p => p.Observe(), Value));
        
    [Pure]
    public void Deconstruct(out K<M, Proxy<UOut, UIn, DIn, DOut, M, A>> value) =>
        value = Value;
    
    [Pure]
    public override string ToString() => 
        "proxyM";
}
