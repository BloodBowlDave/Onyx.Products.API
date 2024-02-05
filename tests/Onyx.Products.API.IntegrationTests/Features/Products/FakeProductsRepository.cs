using Onyx.Products.API.Features.Products;
using Onyx.Products.API.Features.Products.ViewModels;

namespace Onyx.Products.API.IntegrationTests.Features.Products;

public class FakeProductsRepository : IProductsRepository
{
	private readonly List<Product> _products;

	public FakeProductsRepository(List<Product> products)
	{
		_products = products;
	}

	public IList<Product> GetAll()
	{
		return _products;
	}

	public Product? GetById(Guid id)
	{
		return _products.FirstOrDefault(x => x.Id == id);
	}
}