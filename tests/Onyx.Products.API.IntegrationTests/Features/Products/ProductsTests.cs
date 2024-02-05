using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Onyx.Products.API.Features.Products.ViewModels;
using Xunit;

namespace Onyx.Products.API.IntegrationTests.Features.Products
{
	public class ProductsTests : IClassFixture<CustomWebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		private readonly CustomWebApplicationFactory<Program> _factory;

		public ProductsTests(
			CustomWebApplicationFactory<Program> factory)
		{
			_factory = factory;
			_client = factory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
		}

		[Fact]
		public async Task WhenGettingProductsWithNoApiKeyThenReceiveUnauthorizedStatusCode()
		{
			// Arrange
			var request = "/api/products";

			// Act
			var response = await _client.GetAsync(request);

			// Assert
			Assert.Equal(401, (int)response.StatusCode);
		}

		[Fact]
		public async Task WhenGettingProductsWithIncorrectApiKeyThenReceiveUnauthorizedStatusCode()
		{
			// Arrange
			var request = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("/api/products", UriKind.RelativeOrAbsolute)
			};

			request.Headers.Add("x-api-key", "1234");

			// Act
			var response = await _client.SendAsync(request);

			// Assert
			Assert.Equal(401, (int)response.StatusCode);
		}

		[Fact]
		public async Task WhenGettingProductsWithCorrectApiKeyThenReceiveSuccessStatusCodeAndProductsInResponse()
		{
			// Arrange
			var request = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("/api/products", UriKind.RelativeOrAbsolute)
			};

			request.Headers.Add("x-api-key", "e21ed312cc9d4ae79bb57af54dc6acca");

			// Act
			var response = await _client.SendAsync(request);

			// Assert
			Assert.Equal(200, (int)response.StatusCode);

			var responseContent = await response.Content.ReadAsStringAsync();
			var products = JsonConvert.DeserializeObject<List<Product>>(responseContent);

			Assert.NotNull(products);
			Assert.Collection(products, 
				x => Assert.Equal("Test Product", x.Name));
		}
	}
}
