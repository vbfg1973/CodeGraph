using System.Text.Json;
using CodeGraph.Api.Config;
using CodeGraph.Domain;
using CodeGraph.Domain.Graph;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = builder.ConfigureApp();
builder.ConfigureLogging();

Log.Information("Environment: {Environment}", EnvironmentUtility.GetEnvironmentName());
Log.Debug("{Configuration}", builder.Configuration.GetDebugView());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Debug("{AppSettings}", JsonSerializer.Serialize(appSettings));

builder.Services.AddSingleton(appSettings.Database);
builder.Services.AddDatabase();
builder.Services.AddDomainServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();