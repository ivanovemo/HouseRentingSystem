using HouseRentingSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticApiController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticApiController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistic()
        {
            var result = await _statisticService.TotalAsync();

            return Ok(result);
        }
    }
}
