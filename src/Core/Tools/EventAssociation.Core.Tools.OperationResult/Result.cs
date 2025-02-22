namespace EventAssociation.Core.Tools.OperationResult;

using System;
using System.Collections.Generic;
using System.Linq;

public class Result<T>
{
    private readonly T? _value;
    private readonly List<Error>? _errors;
    public bool IsSuccess { get; }

    private Result(T value)
    {
        _value = value;
        _errors = null;
        IsSuccess = true;
    }

    private Result(List<Error> errors)
    {
        if (errors == null || errors.Count == 0)
        {
            throw new ArgumentException("Error list cannot be null or empty", nameof(errors));
        }

        _value = default;
        _errors = errors;
        IsSuccess = false;
    }

    public static Result<T> Ok(T value) => new(value);
    public static Result<T> Err(params Error[] errors) => new(errors.ToList());

    public T Unwrap() =>
        IsSuccess ? _value! : throw new InvalidOperationException("Called Unwrap on an Err result.");

    public List<Error> UnwrapErr() =>
        !IsSuccess ? _errors! : throw new InvalidOperationException("Called UnwrapErr on an Ok result.");

    public bool HasErrors() => !IsSuccess && _errors is not null && _errors.Any();
}

