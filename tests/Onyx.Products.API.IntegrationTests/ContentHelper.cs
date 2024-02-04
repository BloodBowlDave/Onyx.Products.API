using Newtonsoft.Json;
using System.Text;

namespace Onyx.Products.API.IntegrationTests
{
	public static class ContentHelper
	{
		public static StringContent GetStringContent(object obj)
			=> new(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
	}
}
