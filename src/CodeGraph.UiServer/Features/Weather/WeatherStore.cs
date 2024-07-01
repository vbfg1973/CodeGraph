using CodeGraph.UiServer.Data;
using Fluxor;

namespace CodeGraph.UiServer.Features.Weather
{
    public record WeatherState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public WeatherForecast[] Forecasts { get; init; }
    }

    public class WeatherFeature : Feature<WeatherState>
    {
        public override string GetName()
        {
            return "Weather";
        }

        protected override WeatherState GetInitialState()
        {
            return new WeatherState
            {
                Initialized = false,
                Loading = false,
                Forecasts = []
            };
        }
    }

    public static class WeatherReducers 
    {
        [ReducerMethod]
        public static WeatherState OnSetForecasts(WeatherState state, WeatherSetForecastsAction action) 
        {
            return state with 
            {
                Forecasts = action.Forecasts,
                Loading = false
            };
        }

        [ReducerMethod(typeof(WeatherSetInitializedAction))]
        public static WeatherState OnSetInitialized(WeatherState state)
        {
            return state with
            {
                Initialized = true
            };
        }

        [ReducerMethod(typeof(WeatherLoadForecastsAction))]
        public static WeatherState OnLoadForecasts(WeatherState state)
        {
            return state with
            {
                Loading = true
            };
        }
    }

    public class WeatherEffects 
    {
        private readonly ILogger<WeatherEffects> _logger;

        public WeatherEffects(ILogger<WeatherEffects> logger)
        {
            _logger = logger;
        }

        [EffectMethod(typeof(WeatherLoadForecastsAction))]
        public async Task LoadForecasts(IDispatcher dispatcher)
        {
            var weatherService = new WeatherForecastService();
            var forecasts = await weatherService.GetForecastAsync(DateOnly.FromDateTime(DateTime.UtcNow));
            dispatcher.Dispatch(new WeatherSetForecastsAction(forecasts));
        }
    }

    #region WeatherActions
    public class WeatherSetInitializedAction { }
    public class WeatherLoadForecastsAction { }
    public class WeatherSetForecastsAction
    {
        public WeatherForecast[] Forecasts { get; }

        public WeatherSetForecastsAction(WeatherForecast[] forecasts)
        {
            Forecasts = forecasts;
        }
    }
    #endregion
}