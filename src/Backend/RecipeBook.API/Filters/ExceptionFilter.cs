using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeBook.Communication.Responses;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using System.Net;

namespace RecipeBook.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RecipeBookException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknownException(context);
            }

        }

        private void HandleProjectException(ExceptionContext context)
        {
            if (context.Exception is ErrorOnValidationException errorOnValidationException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(errorOnValidationException.Errors));
            }

        }

        private void ThrowUnknownException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));

        }
    }
  
}
