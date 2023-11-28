public static class ChatGPTInterceptorMiddlewareExtensions
{
    public static IApplicationBuilder UseChatGPTInterceptorMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChatGPTInterceptorMiddleware>();
    }
}