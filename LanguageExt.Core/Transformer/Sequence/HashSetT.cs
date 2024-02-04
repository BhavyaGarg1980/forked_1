using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using LanguageExt.TypeClasses;

namespace LanguageExt;

public partial class HashSetT
{
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Arr<B>> Sequence<A, B>(this Arr<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Either<L, B>> Sequence<L, A, B>(this Either<L, A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Identity<B>> Sequence<A, B>(this Identity<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Traverse(Prelude.identity);

    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<IEnumerable<B>> Sequence<A, B>(this IEnumerable<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Lst<B>> Sequence<A, B>(this Lst<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Fin<B>> Sequence<A, B>(this Fin<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Option<B>> Sequence<A, B>(this Option<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Seq<B>> Sequence<A, B>(this Seq<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Set<B>> Sequence<A, B>(this Set<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<HashSet<B>> Sequence<A, B>(this HashSet<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Traverse(Prelude.identity);
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Validation<FAIL, B>> Sequence<FAIL, A, B>(this Validation<FAIL, A> ta, Func<A, HashSet<B>> f) => 
        ta.Map(f).Sequence();
        
    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Validation<MonoidFail, FAIL, B>> Sequence<MonoidFail, FAIL, A, B>(this Validation<MonoidFail, FAIL, A> ta, Func<A, HashSet<B>> f)
        where MonoidFail : Monoid<FAIL>, Eq<FAIL> =>
        ta.Map(f).Traverse(Prelude.identity);

    /// <summary>
    /// Traverses each value in the `ta`, applies it to `f`.  The resulting monadic value is then repeatedly
    /// bound using the monad bind operation, which means the monad laws of the result-type are followed at each
    /// step.  Resulting in a monad that has an inner value of the subject. 
    /// </summary>
    /// <typeparam name="A">Bound value type</typeparam>
    /// <param name="ta">The subject traversable</param>
    /// <param name="f">Mapping and lifting operation</param>
    /// <returns>Mapped and lifted monad</returns>
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HashSet<Eff<B>> Sequence<A, B>(this Eff<A> ta, Func<A, HashSet<B>> f) =>
        ta.Map(f).Traverse(Prelude.identity);
}
