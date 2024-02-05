using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Onyx.Products.API.Features.Products;
using Onyx.Products.API.Features.Products.ViewModels;
using Xunit;

namespace Onyx.Products.API.IntegrationTests.Features.Products
{
	public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		private static readonly Guid TestProductId = new("805d39a9-7df4-4c23-8432-ca4cf82ccb4f");
		private const string TestProductName = "Test Product";
		private readonly List<Product> _products = new()
		{
			new Product
			{
				Id = TestProductId,
				Name = TestProductName,
				Colour = Colour.Blue
			}
		};

		public HealthCheckTests(CustomWebApplicationFactory<Program> factory)
		{
			_client = factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddScoped<IProductsRepository>(x =>
						new FakeProductsRepository(_products));
				});
			}).CreateClient();
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
		public async Task WhenGettingProductsThenReceiveSuccessStatusCodeAndProductsInResponse()
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
				x =>
				{
					Assert.Equal(TestProductId, x.Id);
					Assert.Equal(TestProductName, x.Name);
					Assert.Equal(Colour.Blue, x.Colour);
				});
		}

		[Fact]
		public async Task WhenGettingProductsByInvalidColourThenReceiveBadRequestStatusCode()
		{
			// Arrange
			var request = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("/api/products/by-colour/purple", UriKind.RelativeOrAbsolute)
			};

			request.Headers.Add("x-api-key", "e21ed312cc9d4ae79bb57af54dc6acca");

			// Act
			var response = await _client.SendAsync(request);

			// Assert
			Assert.Equal(400, (int)response.StatusCode);
		}

		[Fact]
		public async Task WhenGettingProductsByColourThenReceiveSuccessStatusCodeAndMatchingProducts()
		{
			// Arrange
			var request = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("/api/products/by-colour/blue", UriKind.RelativeOrAbsolute)
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
				x =>
				{
					Assert.Equal(TestProductId, x.Id);
					Assert.Equal(TestProductName, x.Name);
					Assert.Equal(Colour.Blue, x.Colour);
				});
		}
	}
}
