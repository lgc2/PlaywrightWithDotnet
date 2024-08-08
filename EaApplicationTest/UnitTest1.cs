using AutoFixture.Xunit2;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;
using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Playwright;

namespace EaApplicationTest;

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

		_playwrightDriver.Dispose();
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

		var product = new Product()
		{
			Name = _productCreatePage.GenerateRandomProductName("Test Product"),
			Description = "Test Product Description",
			Price = 65465,
			ProductType = ProductType.EXTERNAL
		};

		await page.GotoAsync("http://localhost:8000/");

		await _productListPage.AccessCreateProductPageAsync();

		await _productCreatePage.CreateProduct(product);
		await _productCreatePage.ClickCreate();

		await _productListPage.ClickOnProductDetailsLnk(product.Name);

		await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);

		_playwrightDriver.Dispose();
	}

	[Theory, AutoData]
	public async Task CreateProductAndAccessTheDetailsTestWithAutofixtureData(Product product)
	{
		var page = await _playwrightDriver.Page;

		await page.GotoAsync("http://localhost:8000/");

		await _productListPage.AccessCreateProductPageAsync();

		await _productCreatePage.CreateProduct(product);
		await _productCreatePage.ClickCreate();

		await _productListPage.ClickOnProductDetailsLnk(product.Name);

		await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);

		_playwrightDriver.Dispose();
	}
}
