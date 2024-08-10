using EaFramework.Driver;
using Microsoft.Playwright;

namespace EaSpecflowTests.Pages;

public interface IProductDetailsPage
{
	ILocator GetPageTitleElement();
	ILocator GetProductNameElement();
}

public class ProductDetailsPage : IProductDetailsPage
{
	private readonly IPage _page;

	public ProductDetailsPage(IPlaywrightDriver playwrightDriver) => _page = playwrightDriver.Page.Result;

	private ILocator _pageTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Details" });
	private ILocator _productName => _page.Locator("#Name");

	public ILocator GetPageTitleElement() => _pageTitle;

	public ILocator GetProductNameElement() => _productName;
}
