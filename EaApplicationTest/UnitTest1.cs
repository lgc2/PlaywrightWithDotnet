using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Playwright;

namespace EaApplicationTest;

public class Tests : IClassFixture<PlaywrightDriverInitializer>
{
	private PlaywrightDriver _playwrightDriver;
	private PlaywrightDriverInitializer _playwrightDriverinitializer;

	public Tests(PlaywrightDriverInitializer playwrightDriverInitializer)
	{
		var testSettings = new TestSettings
		{
			DriverType = DriverType.Firefox,
			Headless = false
		};

		_playwrightDriverinitializer = playwrightDriverInitializer;
		_playwrightDriver = new PlaywrightDriver(testSettings, _playwrightDriverinitializer);

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
}