using Microsoft.OpenApi.Models;
using Onyx.Products.API.Authentication;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddScoped<ApiKeyAuthenticationFilter>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(x =>
{
	x.AddSecurityDefinition("ApiKey",
		new OpenApiSecurityScheme
		{
			Description = "The API Key to access the API",
			Type = SecuritySchemeType.ApiKey,
			Name = "x-api-key",
			In = ParameterLocation.Header,
			Scheme = "ApiKeyScheme"
		});

	var scheme = new OpenApiSecurityScheme
	{
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "ApiKey"
		},
		In = ParameterLocation.Header
	};

	var requirement = new OpenApiSecurityRequirement
	{
		{ scheme, new List<string>() }
	};

	x.AddSecurityRequirement(requirement);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();


app.Run();

public partial class Program { }
