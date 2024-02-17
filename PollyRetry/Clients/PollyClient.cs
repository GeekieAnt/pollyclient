namespace PollyRetry.Clients
{
    public class PollyClient
    {
        private readonly HttpClient _httpClient;

        public PollyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<WeatherForecast>> GetWeatherAsync()
        {
            var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<WeatherForecast>>("/WeatherForecast");
            return items;
        }
    }
}
