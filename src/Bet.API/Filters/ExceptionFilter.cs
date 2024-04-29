using Bet.Application.BaseExceptions;
using Bet.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Bet.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message, "An error occurred.");

            switch (context.Exception)
            {
                case BetException betException:
                    DealBetException(context, betException);
                    break;
                default:
                    ThrowUnknownError(context);
                    break;
            }
        }

        private void DealBetException(ExceptionContext context, BetException exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "Unknown error occurred";

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = notFoundException.Message;
                    break;
                case ConflictException conflictException:
                    statusCode = HttpStatusCode.Conflict;
                    errorMessage = conflictException.Message;
                    break;
                case ValidationErrorException validationErrorException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = string.Join(", ", validationErrorException.ErrorMessages);
                    break;
                case LoginInvalidException loginInvalidException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorMessage = loginInvalidException.Message;
                    break;
            }

            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new ObjectResult(new ErrorResponse(errorMessage));
        }

        private void ThrowUnknownError(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ErrorResponse("Unknown error occurred"));
        }
    }
}
