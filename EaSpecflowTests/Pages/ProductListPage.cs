using EaFramework.Driver;
using Microsoft.Playwright;

namespace EaSpecflowTests.Pages;

public interface IProductListPage
{
	Task AccessCreateProductPageAsync();
	Task ClickOnProductDetailsLnk(string productName);
}

public class ProductListPage : IProductListPage
{
	private readonly IPage _page;

	public ProductListPage(IPlaywrightDriver playwrightDriver) => _page = playwrightDriver.Page.Result;

	private ILocator _lnkProductList => _page.GetByRole(AriaRole.Link, new() { Name = "Product" });
	private ILocator _lnkCreate => _page.GetByRole(AriaRole.Link, new() { Name = "Create" });
	private ILocator _lnkProductDetails(string productName) =>
		_page.GetByRole(AriaRole.Row, new() { Name = $"{productName}" })
			.GetByRole(AriaRole.Link, new() { Name = "Details" });

	public async Task AccessCreateProductPageAsync()
	{
		await _lnkProductList.ClickAsync();
		await _lnkCreate.ClickAsync();
	}

	public async Task ClickOnProductDetailsLnk(string productName)
	{
		await _lnkProductDetails(productName).ClickAsync();
	}
}
