using Microsoft.Playwright;

namespace EaApplicationTest.Pages;

public class ProductDetailsPage
{
	private readonly IPage _page;

	public ProductDetailsPage(IPage page)
	{
		_page = page;
	}

	private ILocator _pageTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Details" });
	private ILocator _productName => _page.Locator("#Name");

	public ILocator GetPageTitleElement() => _pageTitle;

	public ILocator GetProductNameElement() => _productName;
}
