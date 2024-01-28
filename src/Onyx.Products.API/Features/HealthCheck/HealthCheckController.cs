using Microsoft.AspNetCore.Mvc;

namespace Onyx.Products.API.Features.HealthCheck
{
	[ApiController]
	[Route("")]
	public class HealthCheckController : ControllerBase
	{
		[HttpGet(Name = "health-check")]
		public IActionResult HealthCheck()
		{
			return Ok();
		}
	}
}
