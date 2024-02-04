using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LanguageExt.TypeClasses;

namespace LanguageExt;

public partial class ValueTaskT
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
    public static ValueTask<Arr<B>> Sequence<A, B>(this Arr<A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<Either<L, B>> Sequence<L, A, B>(this Either<L, A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<Identity<B>> Sequence<A, B>(this Identity<A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<IEnumerable<B>> SequenceSerial<A, B>(this IEnumerable<A> ta, Func<A, ValueTask<B>> f) =>
        ta.Map(f).SequenceSerial();

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
    public static ValueTask<IEnumerable<B>> SequenceParallel<A, B>(this IEnumerable<A> ta, Func<A, ValueTask<B>> f) =>
        ta.Map(f).SequenceParallel();

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
    public static ValueTask<IEnumerable<B>> SequenceParallel<A, B>(this IEnumerable<A> ta, Func<A, ValueTask<B>> f, int windowSize) =>
        ta.Map(f).SequenceParallel(windowSize);
 
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
    public static ValueTask<ValueTask<B>> Sequence<A, B>(this ValueTask<A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<Fin<B>> Sequence<A, B>(this Fin<A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<Option<B>> Sequence<A, B>(this Option<A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<Seq<B>> SequenceSerial<A, B>(this Seq<A> ta, Func<A, ValueTask<B>> f) =>
        ta.Map(f).SequenceSerial();
        
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
    public static ValueTask<Seq<B>> SequenceParallel<A, B>(this Seq<A> ta, Func<A, ValueTask<B>> f) =>
        ta.Map(f).SequenceParallel();
        
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
    public static ValueTask<Seq<B>> SequenceParallel<A, B>(this Seq<A> ta, Func<A, ValueTask<B>> f, int windowSize) =>
        ta.Map(f).SequenceParallel(windowSize);
        
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
    public static ValueTask<Set<B>> Sequence<A, B>(this Set<A> ta, Func<A, ValueTask<B>> f) =>
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
    public static ValueTask<Validation<FAIL, B>> Sequence<FAIL, A, B>(this Validation<FAIL, A> ta, Func<A, ValueTask<B>> f) => 
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
    public static ValueTask<Validation<MonoidFail, FAIL, B>> Sequence<MonoidFail, FAIL, A, B>(this Validation<MonoidFail, FAIL, A> ta, Func<A, ValueTask<B>> f)
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
    public static ValueTask<Eff<B>> Sequence<A, B>(this Eff<A> ta, Func<A, ValueTask<B>> f) =>
        ta.Map(f).Traverse(Prelude.identity);
}
