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

app.UseChatGPTInterceptorMiddleware();

var proxyGenerator = new ProxyGenerator();

IMyService myService = new MyService();

IMyService proxy = proxyGenerator.CreateInterfaceProxyWithTarget(myService, new TempInterceptor());

// Call the method on the proxy (interception will occur)
proxy.MyMethod();

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
