using System.Text.Json;
using CodeGraph.UiServer.Clients;
using CodeGraph.UiServer.Config;
using CodeGraph.UiServer.Data;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = builder.ConfigureApp();
builder.ConfigureLogging();

Log.Information("Environment: {Environment}", EnvironmentUtility.GetEnvironmentName());
Log.Debug("{Configuration}", builder.Configuration.GetDebugView());

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

Log.Debug("{AppSettings}", JsonSerializer.Serialize(appSettings));

builder.Services.AddSingleton(appSettings.CodeGraphApi);
builder.Services.AddHttpClient<CodeGraphApiClient>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();