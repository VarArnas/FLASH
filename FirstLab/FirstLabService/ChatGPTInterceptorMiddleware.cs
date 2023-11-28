using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class ChatGPTInterceptorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ChatGPTInterceptorMiddleware> _logger;

    public ChatGPTInterceptorMiddleware(RequestDelegate next, ILogger<ChatGPTInterceptorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, OpenAIAPI openai)
    {
        string inputPrompt = await ReadRequestBody(context.Request);
        _logger.LogInformation($"ChatGPT Request: {inputPrompt}");

        await _next(context);

        string outputResult = await ReadResponseBody(context.Response);
        _logger.LogInformation($"ChatGPT Response: {outputResult}");

        SaveLogToFile($"ChatGPT Request: {inputPrompt}");
        SaveLogToFile($"ChatGPT Response: {outputResult}");
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }

    private async Task<string> ReadResponseBody(HttpResponse response)
    {
        var originalBodyStream = response.Body;
        using var memoryStream = new MemoryStream();
        response.Body = memoryStream;

        await _next(response.HttpContext);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(memoryStream).ReadToEnd();
        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalBodyStream);
        response.Body = originalBodyStream;

        return responseBody;
    }
    private void SaveLogToFile(string logMessage)
    {
        string logFilePath = "log.txt";

        try
        {
            File.AppendAllText(logFilePath, $"{DateTime.Now} - {logMessage}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving log to file: {ex.Message}");
        }
    }
}