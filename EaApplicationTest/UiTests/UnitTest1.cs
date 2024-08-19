using System.Reflection;
using AutoFixture.Xunit2;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;
using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Playwright;

namespace EaApplicationTest.UiTests;

public class UnitTest1
{
	private readonly IPlaywrightDriver _playwrightDriver;
	private readonly TestSettings _testSettings;
	private readonly IProductListPage _productListPage;
	private readonly IProductCreatePage _productCreatePage;
	private readonly IProductDetailsPage _productDetailsPage;

	public UnitTest1(
		IPlaywrightDriver playwrightDriver,
		TestSettings testSettings,
		IProductListPage productListPage,
		IProductCreatePage productCreatePage,
		IProductDetailsPage productDetailsPage)
	{
		_playwrightDriver = playwrightDriver;
		_testSettings = testSettings;
		_productListPage = productListPage;
		_productCreatePage = productCreatePage;
		_productDetailsPage = productDetailsPage;
	}

	[Fact]
	public async Task Test1()
	{
		var page = await _playwrightDriver.Page;

		await page.GotoAsync("http://eaapp.somee.com");
		await page.ClickAsync("text=Login");
	}

	[Fact]
	public async Task LoginTest()
	{
		var page = await _playwrightDriver.Page;

		await page.GotoAsync("http://eaapp.somee.com");
		await page.ClickAsync("text=Login");
		await page.GetByLabel("UserName").FillAsync("admin");
		await page.GetByLabel("Password").FillAsync("password");

		await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Log in" }).ClickAsync();
		await page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Employee List" }).ClickAsync();
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
	}
	*/
	[Fact]
	public async Task CreateProductAndAccessTheDetailsTestWithConcreteTypes()
	{
		var page = await _playwrightDriver.Page;

		var product = new Product()
		{
			Name = _productCreatePage.GenerateRandomProductName("Test Product"),
			Description = "Test Product Description",
			Price = 65465,
			ProductType = ProductType.EXTERNAL
		};

		await page.GotoAsync("http://localhost:5001/");

		await _productListPage.AccessCreateProductPageAsync();

		await _productCreatePage.CreateProduct(product);
		await _productCreatePage.ClickCreate();

		await _productListPage.ClickOnProductDetailsLnk(product.Name);

		await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);
	}

	[Theory, AutoData]
	public async Task CreateProductAndAccessTheDetailsTestWithAutofixtureData(Product product)
	{
		var page = await _playwrightDriver.Page;

		await page.GotoAsync("http://localhost:5001/");

		await _productListPage.AccessCreateProductPageAsync();

		await _productCreatePage.CreateProduct(product);
		await _productCreatePage.ClickCreate();

		await _productListPage.ClickOnProductDetailsLnk(product.Name);

		await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);
	}

	[Fact]
	public async Task TestForHar()
	{
		//Playwright
		using var playwright = await Playwright.CreateAsync();
		//Browser
		await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = false
		});
		//Page
		var page = await browser.NewPageAsync(new BrowserNewPageOptions
		{
			RecordHarPath = $"{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}/eapp.har",
			RecordHarUrlFilter = "**/Product/**"
		});

		// await page.RouteFromHARAsync($"{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))}/eapp.har", new PageRouteFromHAROptions
		// {
		// 	Url = "**/Product/List",
		// 	Update = false
		// });

		var product = new Product()
		{
			Name = _productCreatePage.GenerateRandomProductName("Test Product"),
			Description = "Test Product Description",
			Price = 65465,
			ProductType = ProductType.EXTERNAL
		};

		await page.GotoAsync("http://localhost:5001/");

		await page.GetByRole(AriaRole.Link, new() { Name = "Product" }).ClickAsync();

		await page.GetByRole(AriaRole.Link, new() { Name = "Create" }).ClickAsync();

		await page.GetByLabel("Name").FillAsync(product.Name);
		await page.GetByLabel("Description").FillAsync(product.Description);
		await page.Locator("#Price").FillAsync(product.Price.ToString());
		await page.GetByLabel("ProductType").SelectOptionAsync(new[] { product.ProductType.ToString() });
		await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();

		await Task.Delay(1000);

		await page.CloseAsync();
	}
}
