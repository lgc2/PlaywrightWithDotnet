using System.Text.Json;
using EaApplicationTest.Models;
using EaFramework.Driver;
using FluentAssertions;
using Microsoft.Playwright;

namespace EaApplicationTest.ApiTests;

public class ProductsApiTests
{
	private readonly IPlaywrightDriver _playwrightDriver;

	public ProductsApiTests(IPlaywrightDriver playwrightDriver)
	{
		_playwrightDriver = playwrightDriver;
	}

	[Fact]
	public async Task GetProductTest()
	{
		var productCreated = await CreateProductAsync();

		var productResponse = await (await _playwrightDriver.ApiRequestContext).GetAsync($"Product/GetProductById/{productCreated?.Id}");
		var data = await productResponse.JsonAsync();

		var product = data?.Deserialize<Product>(new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		productResponse.Status.Should().Be(200);
		product?.Name.Should().Be(productCreated?.Name);
		product?.Price.Should().Be(productCreated?.Price);
		product?.ProductType.Should().Be(productCreated?.ProductType);
		product?.Id.Should().Be(productCreated?.Id);
	}

	[Fact]
	public async Task DeleteProductTest()
	{
		var product = await CreateProductAsync();

		var deleteResponse = await (await _playwrightDriver.ApiRequestContext).DeleteAsync("Product/Delete",
			new APIRequestContextOptions()
			{
				Params = new Dictionary<string, object>
				{
					{ "id", $"{product?.Id}" }
				}
			});

		var getResponse = await (await _playwrightDriver.ApiRequestContext).GetAsync($"Product/GetProductById/{product?.Id}");

		deleteResponse.Status.Should().Be(200);
		getResponse.Status.Should().Be(204);
	}

	public async Task<Product?> CreateProductAsync()
	{
		var productData = new Product()
		{
			Name = "Test product",
			Description = "Test description",
			Price = 7895,
			ProductType = ProductType.EXTERNAL
		};

		var createResponse = await (await _playwrightDriver.ApiRequestContext).PostAsync("/Product/Create",
			new APIRequestContextOptions()
			{
				Headers = new Dictionary<string, string>
				{
					{ "accept", "text/plain" },
					{ "Content-Type", " application/json-patch+json" }
				},
				DataObject = productData
			});

		var createData = await createResponse.JsonAsync();

		return createData?.Deserialize<Product>(new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});
	}
}
