using System.Text.Json;
using CodeGraph.Clients;
using CodeGraph.UiServer.Config;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using MudBlazor.Services;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = builder.ConfigureApp();
builder.ConfigureLogging();

Log.Information("Environment: {Environment}", EnvironmentUtility.GetEnvironmentName());
Log.Debug("{Configuration}", builder.Configuration.GetDebugView());

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

Log.Debug("{AppSettings}", JsonSerializer.Serialize(appSettings));

builder.Services.AddFluxor(o =>
{
    o.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
    o.UseReduxDevTools();
#endif
});

builder.Services.AddSingleton(appSettings.CodeGraphApi);
builder.Services.AddHttpClient<CodeGraphMethodClient>();
builder.Services.AddHttpClient<CodeGraphFileSystemClient>();

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