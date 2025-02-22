// See https://aka.ms/new-console-template for more information

using EventAssociation.Core.Tools.OperationResult;

Console.WriteLine("Hello, World!");

var success = Result<int>.Ok(42);
Console.WriteLine(success.IsSuccess); // true
Console.WriteLine(success.Unwrap());  // 42

var errorResult = Result<int>.Err(
    new Error("E001", "Invalid input"),
    new Error("E002", "Not found")
);
Console.WriteLine(errorResult.IsSuccess); // false
Console.WriteLine(errorResult.UnwrapErr().Count); // 2

