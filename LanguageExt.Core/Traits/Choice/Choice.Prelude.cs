﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using LanguageExt.Traits;
using static LanguageExt.Prelude;

namespace LanguageExt;

public static class Choice
{
    /// <summary>
    /// Match operation with an untyped value for Some. This can be
    /// useful for serialisation and dealing with the IOptional interface
    /// </summary>
    /// <typeparam name="R">The return type</typeparam>
    /// <param name="Some">Operation to perform if the option is in a Some state</param>
    /// <param name="None">Operation to perform if the option is in a None state</param>
    /// <returns>The result of the match operation</returns>
    [Pure]
    public static R matchUntyped<CHOICE, CH, A, B, R>(CH ma, Func<object?, R> Left, Func<object?, R> Right, Func<R>? Bottom = null)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: x => Left(x),
                     Right: y => Right(y),
                     Bottom: Bottom);

    /// <summary>
    /// Convert the Option to an enumerable of zero or one items
    /// </summary>
    /// <param name="ma">Option</param>
    /// <returns>An enumerable of zero or one items</returns>
    [Pure]
    public static Arr<B> toArray<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: _ => System.Array.Empty<B>(),
                     Right: y => [y],
                     Bottom: System.Array.Empty<B>);

    /// <summary>
    /// Convert the Option to an immutable list of zero or one items
    /// </summary>
    /// <param name="ma">Option</param>
    /// <returns>An immutable list of zero or one items</returns>
    [Pure]
    public static Lst<B> toList<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        toList<B>(toArray<CHOICE, CH, A, B>(ma));

    /// <summary>
    /// Convert the Option to an enumerable sequence of zero or one items
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ma">Option</param>
    /// <returns>An enumerable sequence of zero or one items</returns>
    [Pure]
    public static IEnumerable<B> toSeq<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        toArray<CHOICE, CH, A, B>(ma).AsIterable();

    /// <summary>
    /// Convert the structure to an Either
    /// </summary>
    [Pure]
    public static Either<A, B> toEither<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: l => Left<A, B>(l),
                     Right: r => Right<A, B>(r));

    /// <summary>
    /// Convert the structure to a Option
    /// </summary>
    [Pure]
    public static Option<B> toOption<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: _ => Option<B>.None,
                     Right:      Option<B>.Some,
                     Bottom: () => Option<B>.None);

    [Pure]
    public static B ifLeft<CHOICE, CH, A, B>(CH ma, Func<B> Left)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: _ => Left(),
                     Right: identity);

    [Pure]
    public static B ifLeft<CHOICE, CH, A, B>(CH ma, Func<A, B> leftMap)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: leftMap,
                     Right: identity);

    [Pure]
    public static B ifLeft<CHOICE, CH, A, B>(CH ma, B rightValue)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: _ => rightValue,
                     Right: identity);

    public static Unit ifLeft<CHOICE, CH, A, B>(CH ma, Action<A> Left)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: a => { Left(a); return unit; },
                     Right: ignore,
                     Bottom: () => unit);

    public static Unit ifRight<CHOICE, CH, A, B>(CH ma, Action<B> Right)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: ignore,
                     Right: b => { Right(b); return unit; },
                     Bottom: () => unit);

    /// <summary>
    /// Returns the leftValue if the Either is in a Right state.
    /// Returns the Left value if the Either is in a Left state.
    /// </summary>
    /// <param name="leftValue">Value to return if in the Left state</param>
    /// <returns>Returns an unwrapped Left value</returns>
    [Pure]
    public static A ifRight<CHOICE, CH, A, B>(CH ma, A leftValue)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: identity,
                     Right: _ => leftValue);

    /// <summary>
    /// Returns the result of Right() if the Either is in a Right state.
    /// Returns the Left value if the Either is in a Left state.
    /// </summary>
    /// <param name="Right">Function to generate a Left value if in the Right state</param>
    /// <returns>Returns an unwrapped Left value</returns>
    [Pure]
    public static A ifRight<CHOICE, CH, A, B>(CH ma, Func<A> Right)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: identity,
                     Right: _ => Right());

    /// <summary>
    /// Returns the result of rightMap if the Either is in a Right state.
    /// Returns the Left value if the Either is in a Left state.
    /// </summary>
    /// <param name="rightMap">Function to generate a Left value if in the Right state</param>
    /// <returns>Returns an unwrapped Left value</returns>
    [Pure]
    public static A ifRight<CHOICE, CH, A, B>(CH ma, Func<B, A> rightMap)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: identity,
                     Right: rightMap);

    /// <summary>
    /// Project the Either into a Lst R
    /// </summary>
    /// <returns>If the Either is in a Right state, a Lst of R with one item.  A zero length Lst R otherwise</returns>
    [Pure]
    public static Lst<B> rightToList<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma, 
                     Left: _ => Lst<B>.Empty,
                     Right: b => b.Cons(Lst<B>.Empty),
                     Bottom: () => Lst<B>.Empty);
    
    /// <summary>
    /// Project the Either into an ImmutableArray R
    /// </summary>
    /// <returns>If the Either is in a Right state, a ImmutableArray of R with one item.  A zero length ImmutableArray of R otherwise</returns>
    [Pure]
    public static Arr<B> rightToArray<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        toArray<B>(rightAsEnumerable<CHOICE, CH, A, B>(ma));

    /// <summary>
    /// Project the Either into a Lst R
    /// </summary>
    /// <returns>If the Either is in a Right state, a Lst of R with one item.  A zero length Lst R otherwise</returns>
    [Pure]
    public static Lst<A> leftToList<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: a => a.Cons(Lst<A>.Empty),
                     Right: _ => Lst<A>.Empty,
                     Bottom: () => Lst<A>.Empty);

    /// <summary>
    /// Project the Either into an ImmutableArray R
    /// </summary>
    /// <returns>If the Either is in a Right state, a ImmutableArray of R with one item.  A zero length ImmutableArray of R otherwise</returns>
    [Pure]
    public static Arr<A> leftToArray<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        toArray<A>(leftAsEnumerable<CHOICE, CH, A, B>(ma));

    /// <summary>
    /// Project the Either into a IEnumerable R
    /// </summary>
    /// <returns>If the Either is in a Right state, a IEnumerable of R with one item.  A zero length IEnumerable R otherwise</returns>
    [Pure]
    public static Seq<B> rightAsEnumerable<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma, 
                     Left: _ => Empty,
                     Right: b => b.Cons(Empty),
                     Bottom: () => Empty);

    /// <summary>
    /// Project the Either into a IEnumerable L
    /// </summary>
    /// <returns>If the Either is in a Left state, a IEnumerable of L with one item.  A zero length IEnumerable L otherwise</returns>
    [Pure]
    public static Seq<A> leftAsEnumerable<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: a => a.Cons(Empty),
                     Right: _ => Empty,
                     Bottom: () => Empty);

    [Pure]
    public static int hashCode<CHOICE, CH, A, B>(CH ma)
        where CHOICE : Choice<CH, A, B> =>
        CHOICE.Match(ma,
                     Left: a => a?.GetHashCode()  ?? 0,
                     Right: b => b?.GetHashCode() ?? 0,
                     Bottom: () => -1
        );

    /// <summary>
    /// Extracts from a list of 'Either' all the 'Left' elements.
    /// All the 'Left' elements are extracted in order.
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="ma">Either list</param>
    /// <returns>An enumerable of L</returns>
    [Pure]
    public static IEnumerable<A> lefts<CHOICE, CH, A, B>(IEnumerable<CH> ma)
        where CHOICE : Choice<CH, A, B>
    {
        foreach (var item in ma)
        {
            if (CHOICE.IsLeft(item))
            {
                foreach (var x in CHOICE.Match(
                             item,
                             Left: x => [x],
                             Right: _ => [],
                             Bottom: System.Array.Empty<A>))
                {
                    yield return x;
                }
            }
        }
    }

    /// <summary>
    /// Extracts from a list of 'Either' all the 'Left' elements.
    /// All the 'Left' elements are extracted in order.
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="ma">Either list</param>
    /// <returns>An enumerable of L</returns>
    [Pure]
    public static Seq<A> lefts<CHOICE, CH, A, B>(Seq<CH> ma)
        where CHOICE : Choice<CH, A, B>
    {
        var lst = new List<A>();

        foreach (var item in ma)
        {
            if (CHOICE.IsLeft(item))
            {
                CHOICE.Match(
                    item,
                    Left: x => lst.Add(x),
                    Right: _ => { },
                    Bottom: () => { });
            }
        }

        return Prelude.toSeq(lst);
    }
        
    /// <summary>
    /// Extracts from a list of 'Either' all the 'Right' elements.
    /// All the 'Right' elements are extracted in order.
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="ma">Choice  list</param>
    /// <returns>An enumerable of L</returns>
    [Pure]
    public static IEnumerable<B> rights<CHOICE, CH, A, B>(IEnumerable<CH> ma)
        where CHOICE : Choice<CH, A, B>
    {
        foreach (var item in ma)
        {
            if (CHOICE.IsRight(item))
            {
                foreach (var x in CHOICE.Match(
                             item,
                             Left: _ => [],
                             Right: y => [y],
                             Bottom: System.Array.Empty<B>))
                {
                    yield return x;
                }
            }
        }
    }

    /// <summary>
    /// Extracts from a list of 'Either' all the 'Right' elements.
    /// All the 'Right' elements are extracted in order.
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="ma">Choice  list</param>
    /// <returns>An enumerable of L</returns>
    [Pure]
    public static Seq<B> rights<CHOICE, CH, A, B>(Seq<CH> ma)
        where CHOICE : Choice<CH, A, B>
    {
        var lst = new List<B>();
        foreach (var item in ma)
        {
            if (CHOICE.IsRight(item))
            {
                CHOICE.Match(
                    item,
                    Left: _ => { },
                    Right: y => lst.Add(y),
                    Bottom: () => { });
            }
        }

        return Prelude.toSeq(lst);
    }


    /// <summary>
    /// Partitions a list of 'Either' into two lists.
    /// All the 'Left' elements are extracted, in order, to the first
    /// component of the output.  Similarly the 'Right' elements are extracted
    /// to the second component of the output.
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="ma">Choice list</param>
    /// <returns>A tuple containing the an enumerable of L and an enumerable of R</returns>
    [Pure]
    public static (IEnumerable<A> Lefts, IEnumerable<B> Rights) partition<CHOICE, CH, A, B>(IEnumerable<CH> ma)
        where CHOICE : Choice<CH, A, B>
    {
        var la = ma.ToList();
        return (lefts<CHOICE, CH, A, B>(la), rights<CHOICE, CH, A, B>(la));
    }

    /// <summary>
    /// Partitions a list of 'Either' into two lists.
    /// All the 'Left' elements are extracted, in order, to the first
    /// component of the output.  Similarly the 'Right' elements are extracted
    /// to the second component of the output.
    /// </summary>
    /// <typeparam name="L">Left</typeparam>
    /// <typeparam name="R">Right</typeparam>
    /// <param name="ma">Choice list</param>
    /// <returns>A tuple containing the an enumerable of L and an enumerable of R</returns>
    [Pure]
    public static (Seq<A> Lefts, Seq<B> Rights) partition<CHOICE, CH, A, B>(Seq<CH> ma)
        where CHOICE : Choice<CH, A, B> =>
        (lefts<CHOICE, CH, A, B>(ma), rights<CHOICE, CH, A, B>(ma));
}
