using CodeGraph.Domain;
using CodeGraph.Domain.Graph;
using CodeGraph.Domain.Graph.Database;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Services.AddLogging(config => config.AddSerilog());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

CredentialsConfig credentialsConfig = new("neo4j://localhost:7687;neo4j;neo4j;AdminPassword");
builder.Services.AddSingleton(credentialsConfig);
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