using Serilog;

namespace CodeGraph.Api.Config
{
    public static class ConfigExtensions
    {
        /// <summary>
        ///     Read config from files and env variables and configure app with it
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static AppSettings ConfigureApp(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile($"appsettings.{EnvironmentUtility.GetEnvironmentName()}.json");
            builder.Configuration.AddEnvironmentVariables();
            AppSettings appSettings = new();
            builder.Configuration.Bind("Settings", appSettings);

            return appSettings;
        }

        /// <summary>
        ///     Configure logging for application
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog((ctx, lc) =>
            {
                lc
                    .WriteTo
                    .Console()
                    .ReadFrom.Configuration(ctx.Configuration);
            });

            builder.Services.AddLogging(config => config.AddSerilog());
        }
    }
}