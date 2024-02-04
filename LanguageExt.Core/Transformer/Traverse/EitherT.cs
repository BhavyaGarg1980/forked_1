﻿#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
using System;
using System.Collections.Generic;
using LanguageExt.TypeClasses;
using static LanguageExt.Prelude;

namespace LanguageExt;

public static partial class EitherT
{
    //
    // Collections
    //
        
    public static Either<L, Arr<B>> Traverse<L, A, B>(this Arr<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = 0;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, Arr<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix++;
            }
        }
        return new Arr<B>(res);
    }
                
    public static Either<L, HashSet<B>> Traverse<L, A, B>(this HashSet<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = 0;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, HashSet<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix++;
            }
        }
        return new HashSet<B>(res.AsSpan());
    }
        
    public static Either<L, Lst<B>> Traverse<L, A, B>(this Lst<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = 0;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, Lst<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix++;
            }
        }
        return new Lst<B>(res.AsSpan());
    }
        
    public static Either<L, Que<B>> Traverse<L, A, B>(this Que<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = 0;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, Que<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix++;
            }
        }
        return new Que<B>(res.AsSpan());
    }
        
    public static Either<L, Seq<B>> Traverse<L, A, B>(this Seq<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = 0;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, Seq<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix++;
            }
        }
        return Seq.FromArray(res);
    }
        
    public static Either<L, IEnumerable<B>> Traverse<L, A, B>(this IEnumerable<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new List<B>();
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, IEnumerable<B>>.Left((L)x);
            }
            else
            {
                res.Add(f((A)x));                    
            }
        }
        return Seq.FromArray(res.ToArray());
    }
        
    public static Either<L, Set<B>> Traverse<L, A, B>(this Set<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = 0;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, Set<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix++;
            }
        }
        return new Set<B>(res.AsSpan());
    }
        
    public static Either<L, Stck<B>> Traverse<L, A, B>(this Stck<Either<L, A>> ma, Func<A, B> f)
    {
        var res = new B[ma.Count];
        var ix  = ma.Count - 1;
        foreach (var x in ma)
        {
            if (x.IsLeft)
            {
                return Either<L, Stck<B>>.Left((L)x);
            }
            else
            {
                res[ix] = f((A)x);                    
                ix--;
            }
        }
        return new Stck<B>(res.AsSpan());
    }
        
        
    //
    // Sync types
    //
        
        
    public static Either<L, Either<L, B>> Traverse<L, A, B>(this Either<L, Either<L, A>> ma, Func<A, B> f)
    {
        if (ma.IsLeft)
        {
            return Right(Either<L, B>.Left((L)ma));
        }
        else
        {
            var mb = (Either<L, A>)ma;
            if (mb.IsLeft)
            {
                return Either<L, Either<L, B>>.Left((L)mb);
            }
            else
            {
                return Either<L, Either<L, B>>.Right(f((A)mb));
            }
        }
    }
                
    public static Either<L, Identity<B>> Traverse<L, A, B>(this Identity<Either<L, A>> ma, Func<A, B> f)
    {
        if (ma.Value.IsLeft)
        {
            return Either<L, Identity<B>>.Left((L)ma.Value);
        }
        else
        {
            return Either<L, Identity<B>>.Right(new Identity<B>(f((A)ma.Value)));
        }
    }
        
    public static Either<L, Fin<B>> Traverse<L, A, B>(this Fin<Either<L, A>> ma, Func<A, B> f)
    {
        if (ma.IsFail)
        {
            return Right(ma.Cast<B>());
        }
        else
        {
            var mb = (Either<L, A>)ma;
            if (mb.IsLeft)
            {
                return Either<L, Fin<B>>.Left((L)mb);
            }
            else
            {
                return Either<L, Fin<B>>.Right(f((A)mb));
            }
        }
    }        
        
    public static Either<L, Option<B>> Traverse<L, A, B>(this Option<Either<L, A>> ma, Func<A, B> f)
    {
        if (ma.IsNone)
        {
            return Right(Option<B>.None);
        }
        else
        {
            var mb = (Either<L, A>)ma;
            if (mb.IsLeft)
            {
                return Either<L, Option<B>>.Left((L)mb);
            }
            else
            {
                return Either<L, Option<B>>.Right(f((A)mb));
            }
        }
    }        
        
    public static Either<Fail, Validation<Fail, B>> Traverse<Fail, A, B>(this Validation<Fail, Either<Fail, A>> ma, Func<A, B> f)
    {
        if (ma.IsFail)
        {
            return Right(Validation<Fail, B>.Fail(ma.FailValue));
        }
        else
        {
            var mb = ma.SuccessValue;
            if (mb.IsLeft)
            {
                return Either<Fail, Validation<Fail, B>>.Left((Fail)mb);
            }
            else
            {
                return Either<Fail, Validation<Fail, B>>.Right(f((A)mb));
            }
        }
    }
        
    public static Either<Fail, Validation<MonoidFail, Fail, B>> Traverse<MonoidFail, Fail, A, B>(this Validation<MonoidFail, Fail, Either<Fail, A>> ma, Func<A, B> f) 
        where MonoidFail : Monoid<Fail>, Eq<Fail>
    {
        if (ma.IsFail)
        {
            return Right(Validation<MonoidFail, Fail, B>.Fail(ma.FailValue));
        }
        else
        {
            var mb = ma.SuccessValue;
            if (mb.IsLeft)
            {
                return Either<Fail, Validation<MonoidFail, Fail, B>>.Left((Fail)mb);
            }
            else
            {
                return Either<Fail, Validation<MonoidFail, Fail, B>>.Right(f((A)mb));
            }
        }
    }
        
        
    public static Either<L, Validation<Fail, B>> Traverse<Fail, L, A, B>(this Validation<Fail, Either<L, A>> ma, Func<A, B> f)
    {
        if (ma.IsFail)
        {
            return Right(Validation<Fail, B>.Fail(ma.FailValue));
        }
        else
        {
            var mb = ma.SuccessValue;
            if (mb.IsLeft)
            {
                return Either<L, Validation<Fail, B>>.Left((L)mb);
            }
            else
            {
                return Either<L, Validation<Fail, B>>.Right(f((A)mb));
            }
        }
    }

    public static Either<L, Eff<B>> Traverse<L, A, B>(this Eff<Either<L, A>> ma, Func<A, B> f)
    {
        var tres = ma.Run();
            
        if (tres.IsBottom)
        {
            return Either<L, Eff<B>>.Bottom;
        }
        else if (tres.IsFail)
        {
            return Right(FailEff<B>(tres.Error));
        }
        else if (tres.Value.IsLeft)
        {
            return Either<L, Eff<B>>.Left((L)tres.Value);
        }
        else
        {
            return Either<L, Eff<B>>.Right(SuccessEff(f((A)tres.Value)));
        }
    }
}
