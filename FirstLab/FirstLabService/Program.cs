using Castle.DynamicProxy;
using FirstLabService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<OpenAI_API.OpenAIAPI>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.GPTLogging();

var proxyGenerator = new ProxyGenerator();
ILogService logService = new LogService();
ILogService proxy = proxyGenerator.CreateInterfaceProxyWithTarget(logService, new LogInterceptor());

proxy.LogTime();

app.Run();
