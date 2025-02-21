namespace EventAssociation.Core.Tools.OperationResult;

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public string ErrorType { get;}

    public Error(string code, string message, string errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }

        
}