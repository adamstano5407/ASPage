using APIKros.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace APIKros.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

               await context.Response.WriteAsJsonAsync(new
               {
                   error = context.Response.StatusCode,
                   details = validationException.Errors.Select(e => new
                   {
                       field = e.PropertyName,
                       message = e.ErrorMessage
                   })
               }, cancellationToken);
               
               return true;

                return true;

            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;

            case MissingParentException:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;

            case DataIntegrityException:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
            
            case RuntimeException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    type = "server_error",
                    message = "Unexpected server error."
                }, cancellationToken);

                return true;
        }

        await context.Response.WriteAsJsonAsync(new
        {
            error = context.Response.StatusCode,
            message = exception.Message
        }, cancellationToken);
        
        return true;
    }
}