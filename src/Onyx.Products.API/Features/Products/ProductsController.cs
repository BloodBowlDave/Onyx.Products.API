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

		[HttpGet]
		[Route("by-colour/{colour}")]
		public IActionResult GetProductsByColour(string colour)
		{
			if (!Enum.TryParse(colour, true, out Colour parsedColour))
			{
				return BadRequest();
			}

			var products = _productsRepository.GetByColour(parsedColour);

			return Ok(products);
		}
	}
}
