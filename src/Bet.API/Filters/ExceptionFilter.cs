using Bet.Application.BaseExceptions;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Bet.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An error occurred.");

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
        else if (context.Exception is NotFoundException)
        {
            DealNotFoundException(context);
        }
        else if(context.Exception is ConflictException) 
        {
            DealConfilictException(context);
        }
    }

    private void DealNotFoundException(ExceptionContext context)
    {
        var notFound = context.Exception as NotFoundException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Result = new ObjectResult(new ErrorResponse(notFound.Message));
    }

    private void DealConfilictException(ExceptionContext context)
    {
        var notFound = context.Exception as ConflictException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
        context.Result = new ObjectResult(new ErrorResponse(notFound.Message));
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
