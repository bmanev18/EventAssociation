// See https://aka.ms/new-console-template for more information

using EventAssociation.Core.Tools.OperationResult;

Console.WriteLine("Hello, World!");
var list = new List<Error>();
list.Add(new Error("1234", "1234", "1234"));
list.Add(new Error("1235", "1234", "1234"));
list.Add(new Error("1236", "1234", "1234"));
list.Add(new Error("1237", "1234", "1234"));

var res = Result<int, List<Error>>.Err(list);
// Console.WriteLine(res.Unwrap());
if (res.IsOk)
{
    Console.WriteLine(res.Unwrap());
}
else
{
    foreach (var error in res.UnwrapErr())
    {
        Console.WriteLine(error.Code);
    }

}