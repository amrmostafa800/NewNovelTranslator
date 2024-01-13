using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Responses;

public class Response : IActionResult
{
    protected virtual int statusCode { get; set; }
    public virtual string? Type { get; }
    public virtual string? Description { get; set; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "application/json; charset=utf-8";
        response.StatusCode = statusCode;

        await using (var writer = new StreamWriter(response.Body))
        {
            await writer.WriteAsync(JsonSerializer.Serialize(this));
            await writer.FlushAsync();
        }
    }
}