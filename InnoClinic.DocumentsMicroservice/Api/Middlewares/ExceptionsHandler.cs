using FluentValidation;
using Newtonsoft.Json;
using System.Net;

namespace Api.Middlewares;

public class ExceptionsHandler
{
    private readonly RequestDelegate _next;

    public ExceptionsHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, "internal server error", HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode code)
    {
        var response = context.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)code;

        await response.WriteAsync(JsonConvert.SerializeObject(new { Status = code, Message = message }));
    }
}