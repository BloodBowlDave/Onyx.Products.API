using Xunit;

namespace Onyx.Products.API.IntegrationTests.Features.Products
{
	public class ProductsTests : IClassFixture<TestFixture<Startup>>
	{
		private readonly HttpClient _client;

		public ProductsTests(TestFixture<Startup> fixture)
		{
			_client = fixture.Client;
		}

		[Fact]
		public async Task TestGetStockItemsAsync()
		{
			// Arrange
			var request = "/api/products";

			// Act
			var response = await _client.GetAsync(request);

			// Assert
			response.EnsureSuccessStatusCode();
		}
	}
}
