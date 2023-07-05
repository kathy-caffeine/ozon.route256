using Microsoft.AspNetCore.Http.Extensions;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
using System.Net;
using System.Text;

namespace Route256.Week1.Homework.PriceCalculator.Api.Middlewaries;

public class SmartLogMiddleware
{
    private readonly ILogger<Startup> _logger;
    private readonly RequestDelegate _next;

    public SmartLogMiddleware(
        RequestDelegate next,
        ILogger<Startup> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            string requestBody;
            using (StreamReader sr = new StreamReader(context.Request.Body,
                Encoding.UTF8,
                false,
                1024,
                true))
            {
                requestBody = await sr.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var requestEntity = new RequestLogEntity(
                DateTime.UtcNow,
                context.Request.GetDisplayUrl(),
                ConvertHeadersToString(context.Request.Headers),
                requestBody);

            var originalStream = context.Response.Body;
            await using var memoryResponseStream = new MemoryStream();
            context.Response.Body = memoryResponseStream;

            await _next.Invoke(context);

            memoryResponseStream.Position = 0;
            var responseBody = await new StreamReader(memoryResponseStream).ReadToEndAsync();
            var responseEntity = new ResponseLogEntity(responseBody);

            memoryResponseStream.Position = 0;
            await memoryResponseStream.CopyToAsync(originalStream);
            context.Response.Body = originalStream;
            _logger.LogInformation((new LogEntity(requestEntity, responseEntity)).ToString());
        }
        catch(Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
    private string ConvertHeadersToString(IHeaderDictionary keyValuePairs)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Headers:");
        foreach (var keyValue in keyValuePairs)
        {
            stringBuilder.AppendLine(keyValue.Key.ToString() + ": " + keyValue.Value.ToString());
        }
        return stringBuilder.ToString();
    }
}
