namespace EventAssociation.Core.Tools.OperationResult;
public readonly struct Result<T, E>
{
    private readonly T _value;
    private readonly E _error;
    public bool IsOk { get; }

    private Result(T value)
    {
        _value = value;
        _error = default!;
        IsOk = true;
    }

    private Result(E error)
    {
        _value = default!;
        _error = error;
        IsOk = false;
    }

    public static Result<T, E> Ok(T value) => new(value);
    public static Result<T, E> Err(E error) => new(error);

    public T Unwrap() => 
        IsOk ? _value : throw new InvalidOperationException("Called Unwrap on an Err value.");

    public E UnwrapErr() => 
        !IsOk ? _error : throw new InvalidOperationException("Called UnwrapErr on an Ok value.");
}