using Microsoft.AspNetCore.Mvc;
using PollyRetry.Clients;

namespace PollyRetry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private readonly ILogger<ClientController> _logger;

        private readonly PollyClient _pollyClient;

        public ClientController(ILogger<ClientController> logger, PollyClient pollyClient)
        {
            _logger = logger;
            _pollyClient = pollyClient;

        }




        [HttpGet]
        [Route("PollyTest")]
        public async Task<IActionResult> GetWeather()
        {
            var items = await _pollyClient.GetWeatherAsync();

            return Ok(items);

        }
    }
}
