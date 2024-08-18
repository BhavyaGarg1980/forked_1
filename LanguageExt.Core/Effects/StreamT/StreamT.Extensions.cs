using System;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace LanguageExt;

/// <summary>
/// StreamT extensions
/// </summary>
public static class IterableTExtensions
{
    public static StreamT<M, A> As<M, A>(this K<StreamT<M>, A> ma)
        where M : Monad<M> =>
        (StreamT<M, A>)ma;

    public static MList<A> As<M, A>(this K<MList, A> ma)
        where M : Monad<M> =>
        (MList<A>)ma;

    public static K<M, Option<(A Head, StreamT<M, A> Tail)>> Run<M, A>(this K<StreamT<M>, A> mma)
        where M : Monad<M> =>
        mma.As().Run();

    /// <summary>
    /// Execute the stream's inner monad `M`, combining the results using
    /// its `Alternative<M>.Combine` operator.
    /// </summary>
    /// <param name="mma">Stream to combine</param>
    /// <returns>Result of the combined effects</returns>
    public static K<M, A> Combine<M, A>(this K<StreamT<M>, A> mma) 
        where M : Monad<M>, Alternative<M> =>
        mma.As().runListT.Combine();

    /// <summary>
    /// Execute the stream's inner monad `M`, combining the results using
    /// its `Alternative<M>.Combine` operator.
    /// </summary>
    /// <param name="mma">Stream to combine</param>
    /// <returns>Result of the combined effects</returns>
    static K<M, A> Combine<M, A>(this K<M, MList<A>> mma)
        where M : Monad<M>, Alternative<M> =>
        mma.Bind(ml => ml switch
                       {
                           MNil<A> =>
                               M.Empty<A>(),

                           MCons<M, A>(var head, var tail) =>
                               M.Combine(M.Pure(head), tail.Combine()),

                           MIter<M, A> iter =>
                               M.Combine(M.Pure(iter.Head), iter.TailM().Combine()),

                           _ => throw new NotSupportedException()
                       });

    public static StreamT<M, A> Flatten<M, A>(this K<StreamT<M>, StreamT<M, A>> mma)
        where M : Monad<M> =>
        new StreamMainT<M, A>(mma.As().runListT.Map(ml => ml.Map(ma => ma.runListT)).Flatten());

    public static StreamT<M, A> Flatten<M, A>(this K<StreamT<M>, K<StreamT<M>, A>> mma)
        where M : Monad<M> =>
        new StreamMainT<M, A>(mma.As().runListT.Map(ml => ml.Map(ma => ma.As().runListT)).Flatten());

    public static K<M, MList<A>> Flatten<M, A>(this K<M, MList<K<M, MList<A>>>> mma)
        where M : Monad<M> =>
        mma.Bind(la => la.Flatten());

    public static K<M, MList<A>> Flatten<M, A>(this MList<K<M, MList<A>>> mma)
        where M : Monad<M> =>
        mma switch
        {
            MNil<K<M, MList<A>>>                     => M.Pure(MNil<A>.Default),
            MCons<M, K<M, MList<A>>> (var h, var t)  => h.Append(t.Flatten()),
            MIter<M, K<M, MList<A>>> (var h, _) iter => h.Append(iter.TailM().Flatten()),
            _                                        => throw new NotSupportedException()
        };

    public static K<M, MList<A>> Append<M, A>(this K<M, MList<A>> xs, K<M, MList<A>> ys)
        where M : Monad<M> =>
        xs.Bind(x => x.Append(ys));

    public static StreamT<M, B> Bind<M, A, B>(this Pure<A> ma, Func<A, StreamT<M, B>> f)
        where M : Monad<M> =>
        StreamT<M>.pure(ma.Value).Bind(f);

    public static StreamT<M, B> Bind<M, A, B>(this Pure<A> ma, Func<A, K<StreamT<M>, B>> f)
        where M : Monad<M> =>
        StreamT<M>.pure(ma.Value).Bind(f);

    public static StreamT<M, B> Bind<M, A, B>(this IO<A> ma, Func<A, StreamT<M, B>> f)
        where M : Monad<M> =>
        StreamT<M>.liftIO(ma).Bind(f);

    public static StreamT<M, B> Bind<M, A, B>(this IO<A> ma, Func<A, K<StreamT<M>, B>> f)
        where M : Monad<M> =>
        StreamT<M>.liftIO(ma).Bind(f);

    public static StreamT<M, C> SelectMany<M, A, B, C>(
        this Pure<A> ma,
        Func<A, StreamT<M, B>> bind,
        Func<A, B, C> project)
        where M : Monad<M> =>
        StreamT<M>.pure(ma.Value).SelectMany(bind, project);

    public static StreamT<M, C> SelectMany<M, A, B, C>(
        this Pure<A> ma,
        Func<A, K<StreamT<M>, B>> bind,
        Func<A, B, C> project)
        where M : Monad<M> =>
        StreamT<M>.pure(ma.Value).SelectMany(bind, project);

    public static StreamT<M, C> SelectMany<M, A, B, C>(
        this IO<A> ma,
        Func<A, StreamT<M, B>> bind,
        Func<A, B, C> project)
        where M : Monad<M> =>
        StreamT<M>.liftIO(ma).SelectMany(bind, project);

    public static StreamT<M, C> SelectMany<M, A, B, C>(
        this IO<A> ma,
        Func<A, K<StreamT<M>, B>> bind,
        Func<A, B, C> project)
        where M : Monad<M> =>
        StreamT<M>.liftIO(ma).SelectMany(bind, project);

    /// <summary>
    /// Iterate the stream, ignoring any result.
    /// </summary>
    public static K<M, Unit> Iter<M, A>(this K<StreamT<M>, A> ma) 
        where M : Monad<M> =>
        ma.As().Iter();

    /// <summary>
    /// Retrieve the head of the sequence
    /// </summary>
    public static K<M, Option<A>> Head<M, A>(this K<StreamT<M>, A> ma) 
        where M : Monad<M> =>
        ma.As().Head();

    /// <summary>
    /// Retrieve the head of the sequence
    /// </summary>
    /// <exception cref="ExpectedException">Throws sequence-empty expected-exception</exception>
    public static K<M, A> HeadUnsafe<M, A>(this K<StreamT<M>, A> ma) 
        where M : Monad<M> =>
        ma.As().HeadUnsafe();

    /// <summary>
    /// Retrieve the head of the sequence
    /// </summary>
    public static StreamT<M, A> Tail<M, A>(this K<StreamT<M>, A> ma) 
        where M : Monad<M> =>
        ma.As().Tail();

    /// <summary>
    /// Fold the stream itself, yielding the latest state value when the fold function returns `None`
    /// </summary>
    /// <param name="state">Initial state of the fold</param>
    /// <param name="f">Fold operation</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, S> Fold<M, A,S>(this K<StreamT<M>, A> ma, S state, Func<S, A, Option<S>> f) 
        where M : Monad<M> =>
        ma.As().Fold(state, f);

    /// <summary>
    /// Fold the stream itself, yielding values when the `until` predicate is `true`
    /// </summary>
    /// <param name="state">Initial state of the fold</param>
    /// <param name="f">Fold operation</param>
    /// <param name="until">Predicate</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, S> FoldUntil<M, A, S>(
        this K<StreamT<M>, A> ma, 
        S state, 
        Func<S, A, S> f, 
        Func<S, A, bool> until) 
        where M : Monad<M> =>
        ma.As().FoldUntil(state, f, until);

    /// <summary>
    /// Fold the stream itself, yielding values when the `until` predicate is `true`
    /// </summary>
    /// <param name="state">Initial state of the fold</param>
    /// <param name="f">Fold operation</param>
    /// <param name="until">Predicate</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, S> FoldWhile<M, A,S>(
        this K<StreamT<M>, A> ma,
        S state, 
        Func<S, A, S> f, 
        Func<S, A, bool> @while) 
        where M : Monad<M> =>
        ma.As().FoldWhile(state, f, @while);

    /// <summary>
    /// Left fold
    /// </summary>
    /// <param name="state">Initial state</param>
    /// <param name="f">Folding function</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Accumulate state wrapped in the StreamT inner monad</returns>
    public static K<M, S> FoldM<M, A, S>(
        this K<StreamT<M>, A> ma,
        S state, 
        Func<S, A, K<M, S>> f)
        where M : Monad<M> =>
        ma.As().FoldM(state, f);

    /// <summary>
    /// Concatenate streams
    /// </summary>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, A> Combine<M, A>(
        this K<StreamT<M>, A> first,
        StreamT<M, A> second) 
        where M : Monad<M> =>
        first.As().Combine(second);

    /// <summary>
    /// Concatenate streams
    /// </summary>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, A> Combine<M, A>(
        this K<StreamT<M>, A> first,
        K<StreamT<M>, A> second)
        where M : Monad<M> =>
        first.As().Combine(second);

    /// <summary>
    /// Interleave the items of many streams
    /// </summary>
    /// <param name="first">First stream to merge with</param>
    /// <param name="second">Second stream to merge with</param>
    /// <param name="rest">N streams to merge</param>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, A> Merge<M, A>(
        this K<StreamT<M>, A> first,
        K<StreamT<M>, A> second)
        where M : Monad<M> =>
        first.As().Merge(second);

    /// <summary>
    /// Interleave the items of two streams
    /// </summary>
    /// <param name="first">First stream to merge with</param>
    /// <param name="second">Second stream to merge with</param>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, A> Merge<M, A>(
        this K<StreamT<M>, A> first,
        K<StreamT<M>, A> second, 
        params K<StreamT<M>, A>[] rest)
        where M : Monad<M> =>
        first.As().Merge(second, rest);
 
    /// <summary>
    /// Merge the items of two streams into pairs
    /// </summary>
    /// <param name="first">First stream to merge with</param>
    /// <param name="second">Other stream to merge with</param>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, (A First, B Second)> Zip<M, A, B>(
        this K<StreamT<M>, A> first,
        K<StreamT<M>, B> second)
        where M : Monad<M> =>
        first.As().Zip(second);

    /// <summary>
    /// Merge the items of two streams into 3-tuples
    /// </summary>
    /// <param name="first">First stream to merge with</param>
    /// <param name="second">Second stream to merge with</param>
    /// <param name="third">Third stream to merge with</param>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, (A First, B Second, C Third)> Zip<M, A, B, C>(
        this K<StreamT<M>, A> first,
        K<StreamT<M>, B> second,
        K<StreamT<M>, C> third) 
        where M : Monad<M> =>
        first.As().Zip(second, third);

    /// <summary>
    /// Merge the items of two streams into 4-tuples
    /// </summary>
    /// <param name="first">First stream to merge with</param>
    /// <param name="second">Second stream to merge with</param>
    /// <param name="third">Third stream to merge with</param>
    /// <param name="fourth">Fourth stream to merge with</param>
    /// <returns>Stream transformer</returns>
    public static StreamT<M, (A First, B Second, C Third, D Fourth)> Zip<M, A, B, C, D>(
        this K<StreamT<M>, A> first,
        K<StreamT<M>, B> second,
        K<StreamT<M>, C> third,
        K<StreamT<M>, D> fourth)
        where M : Monad<M> =>
        first.As().Zip(second, third, fourth);
}
