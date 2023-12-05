public static class GPTLoggingExtension
{
    public static IApplicationBuilder GPTLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GPTLogging>();
    }
}