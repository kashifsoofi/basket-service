using Microsoft.AspNetCore.Mvc;

namespace Checkout.BasketService.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
