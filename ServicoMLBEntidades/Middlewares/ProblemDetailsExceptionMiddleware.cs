using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ServicoMLBEntidades.Application.Common.Exceptions;

namespace ServicoMLBEntidades.Middlewares;

public class ProblemDetailsExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProblemDetailsExceptionMiddleware> _logger;

    public ProblemDetailsExceptionMiddleware(RequestDelegate next, ILogger<ProblemDetailsExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            await TratarExcecaoAsync(context, exception);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception exception)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Um ou mais erros de validação ocorreram.",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = string.Join(" ", validationException.Errors.Select(e => e.ErrorMessage)),
            },
            AppException appException => new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{(int)appException.StatusCode}",
                Title = appException.StatusCode.ToString(),
                Status = (int)appException.StatusCode,
                Detail = appException.Message,
            },
            _ => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Ocorreu um erro inesperado.",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "Entre em contato com o suporte se o problema persistir.",
            },
        };

        problemDetails.Instance = context.Request.Path;

        if (problemDetails.Status == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Erro não tratado ao processar {Path}", context.Request.Path);
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
