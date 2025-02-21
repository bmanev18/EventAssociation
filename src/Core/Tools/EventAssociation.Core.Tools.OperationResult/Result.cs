namespace EventAssociation.Core.Tools.OperationResult;

public class Result<T>
{
    public T Value { get; }
    public bool IsSuccess { get; }
}

