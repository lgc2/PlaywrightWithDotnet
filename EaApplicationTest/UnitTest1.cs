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
	}

	[Fact]
	public async Task CreateProductAndAccessTheDetailsTest()
	{
		var page = await _playwrightDriver.Page;
		var productListPage = new ProductListPage(page);
		var productCreatePage = new ProductCreatePage(page);
		var productDetailsPage = new ProductDetailsPage(page);

		var name = productCreatePage.GenerateRandomProductName("UPS");
		var description = "Uninterrupted power supply backup";
		var price = "2000";
		var selectOption = "2";

		await page.GotoAsync("http://localhost:8000/");

		await productListPage.AccessCreateProductPageAsync();

		await productCreatePage.CreateProductAsync(name, description, price, selectOption);

		await productListPage.ClickOnProductDetailsLnk(name);

		await productDetailsPage.ValidateInformations(name);
	}
}
