using System.Linq;
using EaFramework.Driver;
using Microsoft.Playwright;
using ProductAPI.Data;
using ProductAPI.Repository;

namespace EaSpecflowTests.Steps;

[Binding]
public class ReusableSteps
{
	private readonly IPlaywrightDriver _playwrightDriver;
	private readonly ProductDbContext _context;
	private readonly IProductRepository _productRepository;

	public ReusableSteps(IPlaywrightDriver playwrightDriver, ProductDbContext context, IProductRepository productRepository)
	{
		_playwrightDriver = playwrightDriver;
		_context = context;
		_productRepository = productRepository;
	}

	[Given(@"I ensure ""([^""]*)"" data is cleaned up if it already exists")]
	public void GivenIEnsureDataIsCleanedUpIfItAlreadyExists(string productName)
	{
		//await (await _playwrightDriver.ApiRequestContext).DeleteAsync("Product/DeleteByName",
		//new APIRequestContextOptions()
		//{
		//	Params = new Dictionary<string, object>
		//	{
		//			{ "name", $"{productName}" }
		//	}
		//});

		var products = _context.Products.Where(p => p.Name.ToLower() == productName.ToLower()).ToList();

		if (products.Count() > 0)
		{
			foreach (var product in products)
			{
				_productRepository.DeleteProductByName(product.Name);
			}
		}
	}
}
