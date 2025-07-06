using Microsoft.OpenApi.Models;
using QuizLlamaServer;

var builder = WebApplication.CreateBuilder(args);

// Add CORS to allow connection from your phone
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<IGameService, GameService>();

// Add Swagger services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseAllOfForInheritance();
    c.UseOneOfForPolymorphism();
    c.SelectSubTypesUsing(baseType =>
        typeof(Program).Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType))
    );
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizLlama API", Version = "v1" });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizLlama API v1");
    });
}

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add endpoint for SignalR hub
app.MapHub<QuizHub>("/quizhub");
app.MapHub<TestHub>("/testhub");

app.Run();
