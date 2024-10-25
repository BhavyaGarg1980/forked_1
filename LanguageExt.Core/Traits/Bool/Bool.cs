﻿using System.Diagnostics.Contracts;
using LanguageExt.Attributes;

namespace LanguageExt.Traits;

/// <summary>
/// Trait for things that have true and false values.
/// </summary>
[Trait("Bool*")]
public interface Bool<A> : Trait
{
    /// <summary>
    /// Returns True
    /// </summary>
    /// <returns>True</returns>
    [Pure]
    public static abstract A True();

    /// <summary>
    /// Returns False
    /// </summary>
    /// <returns>False</returns>
    [Pure]
    public static abstract A False();

    /// <summary>
    /// Returns the result of the logical AND operation between `a` and `b`
    /// </summary>
    /// <returns>The result of the logical AND operation between `a` and `b`</returns>
    [Pure]
    public static abstract A And(A a, A b);

    /// <summary>
    /// Returns the result of the logical OR operation between `a` and `b`
    /// </summary>
    /// <returns>The result of the logical OR operation between `a` and `b`</returns>
    [Pure]
    public static abstract A Or(A a, A b);

    /// <summary>
    /// Returns the result of the logical NOT operation on `a`
    /// </summary>
    /// <returns>The result of the logical NOT operation on `a`</returns>
    [Pure]
    public static abstract A Not(A a);

    /// <summary>
    /// Returns the result of the logical exclusive-OR operation between `a` and `b`
    /// </summary>
    /// <returns>The result of the logical exclusive-OR operation between `a` and `b`</returns>
    [Pure]
    public static abstract A XOr(A a, A b);

    /// <summary>
    /// Logical implication
    /// </summary>
    /// <returns>If `a` is true that implies `b`, else `true`</returns>
    [Pure]
    public static abstract A Implies(A a, A b);

    /// <summary>
    /// Logical bi-conditional.  Both `a` and `b` must be `true`, or both `a` and `b` must
    /// be false.
    /// </summary>
    /// <returns>`true` if `a == b`, `false` otherwise</returns>
    [Pure]
    public static abstract A BiCondition(A a, A b);
}
