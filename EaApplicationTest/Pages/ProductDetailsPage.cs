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

	public async Task ValidateInformations(string productName)
	{
		await _pageTitle.IsVisibleAsync();

		var productNameText = await _productName.TextContentAsync();
		Assert.Equal(productName, productNameText?.Trim());
	}
}
