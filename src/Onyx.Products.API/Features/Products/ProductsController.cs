using Microsoft.AspNetCore.Mvc;
using Onyx.Products.API.Authentication;

namespace Onyx.Products.API.Features.Products
{
	[ServiceFilter(typeof(ApiKeyAuthenticationFilter))]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductsRepository _productsRepository;

		public ProductsController(IProductsRepository productsRepository)
		{
			_productsRepository = productsRepository;
		}

		[HttpGet]
		public IActionResult GetProducts()
		{
			var products = _productsRepository.GetAll();

			return Ok(products);
		}
	}
}
