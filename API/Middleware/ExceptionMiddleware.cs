using API.Errors;
using System.ComponentModel.DataAnnotations;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
         private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            
            var responseModel = new ApiResponse<object>
            {
                Success = false
            };

            switch (error)
            {
                case CustomException e:
                    response.StatusCode = e.StatusCode;
                    responseModel.Message = e.Message;
                    responseModel.ErrorCode = e.ErrorCode;
                    break;

                case ValidationException e:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    responseModel.Message = "Error de validación";
                    responseModel.ErrorCode = "VALIDATION_ERROR";
                    responseModel.Data = e.Message;
                    break;

                default:
                    _logger.LogError(error, error.Message);
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    responseModel.Message = "Ha ocurrido un error interno del servidor";
                    responseModel.ErrorCode = "INTERNAL_SERVER_ERROR";
                    break;
            }

            await response.WriteAsJsonAsync(responseModel);
        }
    }
    }
}
