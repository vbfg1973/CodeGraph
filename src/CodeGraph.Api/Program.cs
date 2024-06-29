using System.Text.Json;
using System.Text.Json.Serialization;
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

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

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