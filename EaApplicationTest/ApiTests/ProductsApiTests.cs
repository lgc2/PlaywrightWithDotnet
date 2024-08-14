using System.Text.Json;
using EaApplicationTest.Models;
using EaFramework.Driver;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EaApplicationTest.ApiTests;

public class ProductsApiTests
{
	private readonly IPlaywrightDriver _playwrightDriver;

	public ProductsApiTests(IPlaywrightDriver playwrightDriver)
	{
		_playwrightDriver = playwrightDriver;
	}

	[Fact]
	public async Task GetProduct()
	{
		var productResponse = await (await _playwrightDriver.ApiRequestContext).GetAsync("Product/GetProductById/1");
		var data = await productResponse.JsonAsync();

		var product = data?.Deserialize<Product>(new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		using (new AssertionScope())
		{
			productResponse.Status.Should().Be(200);
			product?.Name.Should().Be("Keyboard");
			product?.Price.Should().Be(150);
			product?.Id.Should().Be(1);
		}
	}
}
