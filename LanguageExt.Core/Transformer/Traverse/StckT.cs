﻿#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
using System;
using System.Linq;
using System.Collections.Generic;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace LanguageExt;

public static partial class StckT
{
    public static Stck<Arr<B>> Traverse<A, B>(this Arr<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.Map(xs => xs.ToList()).ToArray(), f)
                     .Map(toArray));
        
    public static Stck<Either<L, B>> Traverse<L, A, B>(this Either<L, Stck<A>> ma, Func<A, B> f) =>
        ma.Match(
            Left: ex => Stack(Either<L, B>.Left(ex)),
            Right: xs => xs.Map(x => Right<L, B>(f(x))));

    public static Stck<HashSet<B>> Traverse<A, B>(this HashSet<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.ToArray().Map(xs => xs.ToList()).ToArray(), f)
                     .Map(toHashSet));

    public static Stck<Identity<B>> Traverse<A, B>(this Identity<Stck<A>> ma, Func<A, B> f) =>
        toStack(ma.Value.Map(a => new Identity<B>(f(a))));

    public static Stck<Lst<B>> Traverse<A, B>(this Lst<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.Map(xs => xs.ToList()).ToArray(), f)
                     .Map(toList));

    public static Stck<Fin<B>> Traverse<A, B>(this Fin<Stck<A>> ma, Func<A, B> f) =>
        ma.Match(
            Fail: er => Stack(FinFail<B>(er)),
            Succ: xs => xs.Map(x => FinSucc(f(x))));

    public static Stck<Option<B>> Traverse<A, B>(this Option<Stck<A>> ma, Func<A, B> f) =>
        ma.Match(
            None: () => Stack(Option<B>.None),
            Some: xs => xs.Map(x => Some(f(x))));

    public static Stck<Que<B>> Traverse<A, B>(this Que<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.Map(xs => xs.ToList()).ToArray(), f)
                     .Map(toQueue));

    public static Stck<Seq<B>> Traverse<A, B>(this Seq<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.Map(xs => xs.ToList()).ToArray(), f)
                     .Map(toSeq));

    public static Stck<IEnumerable<B>> Traverse<A, B>(this IEnumerable<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.Map(xs => xs.ToList()).ToArray(), f)
                     .Map(xs => xs.AsEnumerable()));

    public static Stck<Set<B>> Traverse<A, B>(this Set<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.ToArray().Map(xs => xs.ToList()).ToArray(), f)
                     .Map(toSet));

    public static Stck<Stck<B>> Traverse<A, B>(this Stck<Stck<A>> ma, Func<A, B> f) =>
        toStack(CollT.AllCombinationsOf(ma.Reverse().Map(xs => xs.Reverse().ToList()).ToArray(), f)
                     .Map(toStack));

    public static Stck<Validation<Fail, B>> Traverse<Fail, A, B>(this Validation<Fail, Stck<A>> ma, Func<A, B> f) =>
        ma.Match(
            Fail: ex => Stack(Validation<Fail, B>.Fail(ex)),
            Succ: xs => xs.Map(x => Success<Fail, B>(f(x))));

    public static Stck<Validation<MonoidFail, Fail, B>> Traverse<MonoidFail, Fail, A, B>(this Validation<MonoidFail, Fail, Stck<A>> ma, Func<A, B> f) 
        where MonoidFail : Monoid<Fail>, Eq<Fail> =>
        ma.Match(
            Fail: ex => Stack(Validation<MonoidFail, Fail, B>.Fail(ex)),
            Succ: xs => xs.Map(x => Success<MonoidFail, Fail, B>(f(x))));

    public static Stck<Eff<B>> Traverse<A, B>(this Eff<Stck<A>> ma, Func<A, B> f) =>
        ma.Match(
            Fail: ex => Stack(FailEff<B>(ex)),
            Succ: xs => xs.Map(x => SuccessEff(f(x)))).Run().Value;    
}
