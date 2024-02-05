using Onyx.Products.API.Features.Products.ViewModels;

namespace Onyx.Products.API.Features.Products;

public interface IProductsRepository
{
	public IList<Product> GetAll();
	public IList<Product> GetByColour(Colour colour);
}