using Castle.DynamicProxy;
using Castle.Windsor;
using FirstLabService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class GPTLogging
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GPTLogging> _logger;

    public GPTLogging(RequestDelegate next, ILogger<GPTLogging> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string inputPrompt = context.Request.Method == "POST" ? await ReadRequestBody(context.Request) : context.Request.QueryString.ToString();

        inputPrompt = inputPrompt.Replace("+", "PLACEHOLDER_FOR_PLUS");
        inputPrompt = inputPrompt.Replace("-", "PLACEHOLDER_FOR_MINUS");
        inputPrompt = inputPrompt.Replace("/", "PLACEHOLDER_FOR_DIVISION");
        inputPrompt = inputPrompt.Replace("*", "PLACEHOLDER_FOR_MULTIPLICATION");

        inputPrompt = WebUtility.UrlDecode(inputPrompt);
        inputPrompt = inputPrompt.Replace("%", " ");

        inputPrompt = inputPrompt.Replace("PLACEHOLDER_FOR_PLUS", "+");
        inputPrompt = inputPrompt.Replace("PLACEHOLDER_FOR_MINUS", "-");
        inputPrompt = inputPrompt.Replace("PLACEHOLDER_FOR_DIVISION", "/");
        inputPrompt = inputPrompt.Replace("PLACEHOLDER_FOR_MULTIPLICATION", "*");
        inputPrompt = inputPrompt.Substring(7);

        _logger.LogInformation($"ChatGPT Request: {inputPrompt}");

        await _next(context);

        SaveLogToFile($"ChatGPT Request: {inputPrompt}");
    }


    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
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