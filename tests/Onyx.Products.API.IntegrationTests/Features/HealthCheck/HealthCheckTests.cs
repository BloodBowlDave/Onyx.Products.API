using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Onyx.Products.API.IntegrationTests.Features.HealthCheck
{
	public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;

		public HealthCheckTests(CustomWebApplicationFactory<Program> factory)
		{
			_client = factory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
		}

		[Fact]
		public async Task WhenGettingHealthCheckThenGetSuccessStatusCode()
		{
			// Arrange
			var request = "/";

			// Act
			var response = await _client.GetAsync(request);

			// Assert
			Assert.Equal(200, (int)response.StatusCode);
		}
	}
}
