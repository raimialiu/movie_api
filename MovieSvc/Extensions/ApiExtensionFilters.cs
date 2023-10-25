using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieSvc.Application.Common.Model;

namespace MovieSvc.Extensions;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var response = context.Exception is ValidationException
            ? ProcessValidationErrors(context)
            : ProcessSystemErrors(context);
        context.Result = new JsonResult(response);
    }

    private static ResponseModel ProcessSystemErrors(ExceptionContext context)
    {
        var message = context.Exception.Message;

        return ResponseModel<Exception>.Failure(context.Exception, message);
    }

    private ResponseModel ProcessValidationErrors(ExceptionContext context)
    {
        var validationErrors = context.Exception.Message;
        //var message = validationErrors.FirstOrDefault().Value.FirstOrDefault();
        return ResponseModel.Failure(validationErrors);
    }
}

public class CustomModelValidate : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var message = string.Join(",",
                context.ModelState.Values.Select(e =>
                    string.Join(",", e.Errors.Select(RemoveExtraMessage))));

            context.Result = new BadRequestObjectResult(ResponseModel.Failure(message));
        }
    }

 
    private string RemoveExtraMessage(ModelError error)
    {
        if (!(error.ErrorMessage.IndexOf(" Path ") > 0))
            return error.ErrorMessage;

        return error.ErrorMessage.Remove(error.ErrorMessage.IndexOf(" Path "),
            error.ErrorMessage.Length - error.ErrorMessage.IndexOf(" Path "));
    }
}