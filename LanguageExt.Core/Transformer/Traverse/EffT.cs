#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using LanguageExt.Common;
using LanguageExt.Effects;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace LanguageExt;

public static partial class EffT
{
    //
    // Collections
    //

    [Pure]
    public static Eff<Arr<B>> Traverse<A, B>(this Arr<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<Arr<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(new Arr<B>(rs.ToArray()));
             });

    [Pure]
    public static Eff<HashSet<B>> Traverse<A, B>(this HashSet<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<HashSet<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(new HashSet<B>(rs));
             });

    [Pure]
    public static Eff<IEnumerable<B>> Traverse<A, B>(this IEnumerable<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<IEnumerable<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(rs.AsEnumerable());
             });

    [Pure]
    public static Eff<Lst<B>> Traverse<A, B>(this Lst<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<Lst<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(rs.Freeze());
             });


    [Pure]
    public static Eff<Que<B>> Traverse<A, B>(this Que<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<Que<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(toQueue(rs));
             });

    [Pure]
    public static Eff<Seq<B>> Traverse<A, B>(this Seq<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<Seq<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(Seq.FromArray(rs.ToArray()));
             });

    [Pure]
    public static Eff<Set<B>> Traverse<A, B>(this Set<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<Set<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }

                 return FinSucc(toSet(rs));
             });


    [Pure]
    public static Eff<Stck<B>> Traverse<A, B>(this Stck<Eff<A>> ma, Func<A, B> f) =>
        lift((MinRT rt) =>
             {
                 var rs = new List<B>();
                 foreach (var m in ma)
                 {
                     if (rt.CancellationToken.IsCancellationRequested) return Errors.Cancelled;
                     var r = m.Run();
                     if (r.IsFail) return FinFail<Stck<B>>(r.Error);
                     rs.Add(f(r.Value));
                 }
                 return FinSucc(toStack(rs));
             });
        
        
    //
    // Sync types
    // 
        
    public static Eff<Either<L, B>> Traverse<L, A, B>(this Either<L, Eff<A>> ma, Func<A, B> f)
    {
        return lift(() => Go(ma, f));
        Fin<Either<L, B>> Go(Either<L, Eff<A>> ma, Func<A, B> f)
        {
            if(ma.IsBottom) return default;
            if(ma.IsLeft) return FinSucc<Either<L, B>>(ma.LeftValue);
            var rb = ma.RightValue.Run();
            if(rb.IsFail) return FinFail<Either<L, B>>(rb.Error);
            return FinSucc<Either<L, B>>(f(rb.Value));
        }
    }

    public static Eff<Identity<B>> Traverse<A, B>(this Identity<Eff<A>> ma, Func<A, B> f)
    {
        return lift(() => Go(ma, f));

        Fin<Identity<B>> Go(Identity<Eff<A>> ma, Func<A, B> f)
        {
            if (ma.IsBottom) return default;
            var rb = ma.Value.Run();
            if (rb.IsFail) return FinFail<Identity<B>>(rb.Error);
            return FinSucc(new Identity<B>(f(rb.Value)));
        }
    }

    public static Eff<Fin<B>> Traverse<A, B>(this Fin<Eff<A>> ma, Func<A, B> f)
    {
        return lift(() => Go(ma, f));
        Fin<Fin<B>> Go(Fin<Eff<A>> ma, Func<A, B> f)
        {
            if(ma.IsFail) return FinSucc(ma.Cast<B>());
            var rb = ma.Value.Run();
            if(rb.IsFail) return FinFail<Fin<B>>(rb.Error);
            return FinSucc(Fin<B>.Succ(f(rb.Value)));
        }
    }
        
    public static Eff<Option<B>> Traverse<A, B>(this Option<Eff<A>> ma, Func<A, B> f)
    {
        return lift(() => Go(ma, f));
        Fin<Option<B>> Go(Option<Eff<A>> ma, Func<A, B> f)
        {
            if(ma.IsNone) return FinSucc<Option<B>>(None);
            var rb = ma.Value.Run();
            if(rb.IsFail) return FinFail<Option<B>>(rb.Error);
            return FinSucc(Option<B>.Some(f(rb.Value)));
        }
    }

    public static Eff<Validation<Fail, B>> Traverse<Fail, A, B>(this Validation<Fail, Eff<A>> ma, Func<A, B> f)
    {
        return lift(() => Go(ma, f));
        Fin<Validation<Fail, B>> Go(Validation<Fail, Eff<A>> ma, Func<A, B> f)
        {
            if (ma.IsFail) return FinSucc(Fail<Fail, B>(ma.FailValue));
            var rb = ma.SuccessValue.Run();
            if(rb.IsFail) return FinFail<Validation<Fail, B>>(rb.Error);
            return FinSucc<Validation<Fail, B>>(f(rb.Value));
        }
    }
        
    public static Eff<Validation<MonoidFail, Fail, B>> Traverse<MonoidFail, Fail, A, B>(this Validation<MonoidFail, Fail, Eff<A>> ma, Func<A, B> f)
        where MonoidFail : Monoid<Fail>, Eq<Fail>
    {
        return lift(() => Go(ma, f));
        Fin<Validation<MonoidFail, Fail, B>> Go(Validation<MonoidFail, Fail, Eff<A>> ma, Func<A, B> f)
        {
            if (ma.IsFail) return FinSucc(Fail<MonoidFail, Fail, B>(ma.FailValue));
            var rb = ma.SuccessValue.Run();
            if(rb.IsFail) return FinFail<Validation<MonoidFail, Fail, B>>(rb.Error);
            return FinSucc<Validation<MonoidFail, Fail, B>>(f(rb.Value));
        }
    }
}
