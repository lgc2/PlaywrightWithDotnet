using AutoFixture.Xunit2;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;
using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Playwright;

namespace EaApplicationTest;

public class Tests : IClassFixture<PlaywrightDriverInitializer>
{
	private PlaywrightDriver _playwrightDriver;
	private PlaywrightDriverInitializer _playwrightDriverinitializer;
	private TestSettings _testSettings;

	public Tests(PlaywrightDriverInitializer playwrightDriverInitializer)
	{
		_testSettings = ConfigReader.ReadConfig();

		_playwrightDriverinitializer = playwrightDriverInitializer;
		_playwrightDriver = new PlaywrightDriver(_testSettings, _playwrightDriverinitializer);

	}

	[Fact]
	public async Task Test1()
	{
		var page = await _playwrightDriver.Page;

		await page.GotoAsync(_testSettings.ApplicationUrl);
		await page.ClickAsync("text=Login");

		_playwrightDriver.Dispose();
	}

	[Fact]
	public async Task LoginTest()
	{
		var page = await _playwrightDriver.Page;

		await page.GotoAsync(_testSettings.ApplicationUrl);
		await page.ClickAsync("text=Login");
		await page.GetByLabel("UserName").FillAsync("admin");
		await page.GetByLabel("Password").FillAsync("password");

		await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
		await page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Employee List" }).ClickAsync();

		_playwrightDriver.Dispose();
	}
	/*
	[Theory]
	[InlineData("UPS", "Uninterrupted power supply backup", 2063, 2)]
	[InlineData("Monitor LG", "Monitor of 24\"", 20099, 1)]
	public async Task CreateProductAndAccessTheDetailsTestWithInlineData(
		string name,
		string description,
		int price,
		int selectOption)
	{
		var page = await _playwrightDriver.Page;
		var productListPage = new ProductListPage(page);
		var productCreatePage = new ProductCreatePage(page);
		var productDetailsPage = new ProductDetailsPage(page);

		name = productCreatePage.GenerateRandomProductName(name);

		await page.GotoAsync("http://localhost:8000/");

		await productListPage.AccessCreateProductPageAsync();

		await productCreatePage.CreateProduct(name, description, price, selectOption);
		await productCreatePage.ClickCreate();

		await productListPage.ClickOnProductDetailsLnk(name);

		await Assertions.Expect(productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(productDetailsPage.GetProductNameElement()).ToHaveTextAsync(name);

		_playwrightDriver.Dispose();
	}
	*/
	[Fact]
	public async Task CreateProductAndAccessTheDetailsTestWithConcreteTypes()
	{
		var page = await _playwrightDriver.Page;
		var productListPage = new ProductListPage(page);
		var productCreatePage = new ProductCreatePage(page);
		var productDetailsPage = new ProductDetailsPage(page);

		var product = new Product()
		{
			Name = productCreatePage.GenerateRandomProductName("Test Product"),
			Description = "Test Product Description",
			Price = 65465,
			ProductType = ProductType.EXTERNAL
		};

		await page.GotoAsync("http://localhost:8000/");

		await productListPage.AccessCreateProductPageAsync();

		await productCreatePage.CreateProduct(product);
		await productCreatePage.ClickCreate();

		await productListPage.ClickOnProductDetailsLnk(product.Name);

		await Assertions.Expect(productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);

		_playwrightDriver.Dispose();
	}

	[Theory, AutoData]
	public async Task CreateProductAndAccessTheDetailsTestWithAutofixtureData(Product product)
	{
		var page = await _playwrightDriver.Page;
		var productListPage = new ProductListPage(page);
		var productCreatePage = new ProductCreatePage(page);
		var productDetailsPage = new ProductDetailsPage(page);

		await page.GotoAsync("http://localhost:8000/");

		await productListPage.AccessCreateProductPageAsync();

		await productCreatePage.CreateProduct(product);
		await productCreatePage.ClickCreate();

		await productListPage.ClickOnProductDetailsLnk(product.Name);

		await Assertions.Expect(productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);

		_playwrightDriver.Dispose();
	}
}
