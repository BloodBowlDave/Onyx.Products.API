using Microsoft.AspNetCore.Mvc;
using Onyx.Products.API.Authentication;
using Onyx.Products.API.Features.Products.ViewModels;

namespace Onyx.Products.API.Features.Products
{
	[ServiceFilter(typeof(ApiKeyAuthenticationFilter))]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetProducts()
		{
			var products = new List<Product>
			{
				new Product()
				{
					Id = Guid.NewGuid(),
					Name = "Test Product"
				}
			};

			return Ok(products);
		}
	}
}
