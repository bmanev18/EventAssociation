using EventAssociation.Core.Tools.OperationResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.ActionResultHelper;

public class ActionResultConverter
{
    
    public static ActionResult FromErrors(IEnumerable<Error> errors) =>
        new JsonResult(new
        {
            success = false,
            errors = errors.Select(e => new { e.Code, e.Message, e.ErrorType })
        });
}