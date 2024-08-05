using EaApplicationTest.Models;
using Microsoft.Playwright;

namespace EaApplicationTest.Pages;

public class ProductCreatePage
{
	private readonly IPage _page;

	public ProductCreatePage(IPage page)
	{
		_page = page;
	}

	private ILocator _iptName => _page.GetByLabel("Name");
	private ILocator _iptDescription => _page.GetByLabel("Description");
	private ILocator _iptPrice => _page.Locator("#Price");
	private ILocator _selectProductType => _page.GetByLabel("ProductType");
	private ILocator _btnCreate => _page.GetByRole(AriaRole.Button, new() { Name = "Create" });

	public async Task CreateProduct(Product product)
	{
		await _iptName.FillAsync(product.Name);
		await _iptDescription.FillAsync(product.Description);
		await _iptPrice.FillAsync(product.Price.ToString());
		await _selectProductType.SelectOptionAsync(new[] { product.ProductType.ToString() });

	}

	public async Task ClickCreate() => await _btnCreate.ClickAsync();

	public string GenerateRandomProductName(string name)
	{

		var randon = new Random();
		var randonNumber = randon.Next(10, 10000);

		return $"[{randonNumber}] {name}";
	}
}
