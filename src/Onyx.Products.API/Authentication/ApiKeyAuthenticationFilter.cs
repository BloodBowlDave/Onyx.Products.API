using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Onyx.Products.API.Authentication
{
	public class ApiKeyAuthenticationFilter : Attribute, IAuthorizationFilter
	{
		private readonly IConfiguration _configuration;

		public ApiKeyAuthenticationFilter(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!context.HttpContext.Request.Headers.TryGetValue(AuthenticationConstants.ApiKeyHeaderName, out
				    var extractedApiKey))
			{
				context.Result = new UnauthorizedObjectResult("API Key Missing");
				return;
			}

			var apiKey = _configuration.GetValue<string>(AuthenticationConstants.ApiKeySectionName);
			if (!extractedApiKey.Equals(apiKey))
			{
				context.Result = new UnauthorizedObjectResult("Invalid API Key");
			}
		}
	}
}
