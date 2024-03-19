using Bet.Application.BaseExceptions;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Bet.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BetException)
        {
            DealBetExceptions(context);
        }
        else
        {
            ThrowUnkownError(context);
        }
    }

    private void DealBetExceptions(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorException)
        {
            DealValidationErrorsException(context);
        }
        else if (context.Exception is LoginInvalidException)
        {
            DealLoginException(context);
        }
    }

    private void DealValidationErrorsException(ExceptionContext context)
    {
        var validationErrorException = context.Exception as ValidationErrorException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ErrorResponse(validationErrorException.ErrorMessages));
    }
    private void DealLoginException(ExceptionContext context)
    {
        var erroLogin = context.Exception as LoginInvalidException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new ErrorResponse(erroLogin.Message));
    }

    private void ThrowUnkownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ErrorResponse("Erro desconhecido"));
    }
}
