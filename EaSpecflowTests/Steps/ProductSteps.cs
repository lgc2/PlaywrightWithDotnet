using EaSpecflowTests.Models;
using EaSpecflowTests.Pages;
using Microsoft.Playwright;
using ProductAPI.Repository;
using TechTalk.SpecFlow.Assist;

namespace EaSpecflowTests.Steps;

[Binding]
public sealed class ProductSteps
{
	private readonly ScenarioContext _scenarioContext;
	private readonly IProductCreatePage _productCreatePage;
	private readonly IProductListPage _productListPage;
	private readonly IProductDetailsPage _productDetailsPage;
	private readonly IProductRepository _productRepository;

	public ProductSteps(ScenarioContext scenarioContext,
		IProductCreatePage productCreatePage,
		IProductListPage productListPage,
		IProductDetailsPage productDetailsPage,
		IProductRepository productRepository)
	{
		_scenarioContext = scenarioContext;
		_productCreatePage = productCreatePage;
		_productListPage = productListPage;
		_productDetailsPage = productDetailsPage;
		_productRepository = productRepository;
	}

	[Given(@"I access the create product page")]
	public async Task GivenIAccessTheCreateProductPage()
	{
		await _productListPage.AccessCreateProductPageAsync();
	}

	[Given(@"I create a product with the following details")]
	[When(@"I create a product with the following details")]
	public async Task GivenICreateAProductWithTheFollowingDetails(Table table)
	{
		var product = table.CreateInstance<Product>();

		await _productCreatePage.CreateProduct(product);
		await _productCreatePage.ClickCreate();

		_scenarioContext.Set(product);
	}

	[When(@"I click the details link of the newly created product")]
	public async Task WhenIClickTheDetailsLinkOfTheNewlyCreatedProduct()
	{
		var product = _scenarioContext.Get<Product>();
		await _productListPage.ClickOnProductDetailsLnk(product.Name);
	}

	[Then(@"I see that all the product details are created as expected")]
	public async Task ThenTheISeeThatAllTheProductDetailsAreCreatedAsExpected()
	{
		var product = _scenarioContext.Get<Product>();

		await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
		await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);
	}

	[Given(@"I ensure to count the total number of products from DB")]
	public void GivenIEnsureToCountTheTotalNumberOfProductsFromDB()
	{
		var numberOfProducts = _productRepository.GetProductsCount();
		_scenarioContext.Set(numberOfProducts, "numberOfProducts");
	}

	[Then(@"I see the count of products increments")]
	public void ThenISeeTheCountOfProductsIncrements()
	{
		var expectedNumberOfProducts = _scenarioContext.Get<int>("numberOfProducts") + 1;
		var updatedNumberOfProducts = _productRepository.GetProductsCount();
		Assert.True(expectedNumberOfProducts == updatedNumberOfProducts, $"The number of products should be {expectedNumberOfProducts}.");
	}

}
