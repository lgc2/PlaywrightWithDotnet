using AutoFixture.Xunit2;
using EaApplicationTest.Fixture;
using EaApplicationTest.Models;
using EaApplicationTest.Pages;
using Microsoft.Playwright;

namespace EaApplicationTest.UiTests;

public class CreateProductTest
{
    private readonly ITestFixtureBase _testFixtureBase;
    private readonly IProductListPage _productListPage;
    private readonly IProductCreatePage _productCreatePage;
    private readonly IProductDetailsPage _productDetailsPage;

    public CreateProductTest(
        ITestFixtureBase testFixtureBase,
        IProductListPage productListPage,
        IProductCreatePage productCreatePage,
        IProductDetailsPage productDetailsPage)
    {
        _testFixtureBase = testFixtureBase;
        _productListPage = productListPage;
        _productCreatePage = productCreatePage;
        _productDetailsPage = productDetailsPage;
    }

    [Fact]
    public async Task CreateProductAndAccessTheDetailsTestWithConcreteTypes()
    {
        var product = new Product()
        {
            Name = _productCreatePage.GenerateRandomProductName("Test Product"),
            Description = "Test Product Description",
            Price = 65465,
            ProductType = ProductType.EXTERNAL
        };

        await _testFixtureBase.NavigateToUrl();

        await _productListPage.AccessCreateProductPageAsync();

        await _productCreatePage.CreateProduct(product);
        await _productCreatePage.ClickCreate();

        await _productListPage.ClickOnProductDetailsLnk(product.Name);

        await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
        await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
        await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);
    }

    [Theory, AutoData]
    public async Task CreateProductAndAccessTheDetailsTestWithAutofixtureData(Product product)
    {
        await _testFixtureBase.NavigateToUrl();

        await _productListPage.AccessCreateProductPageAsync();

        await _productCreatePage.CreateProduct(product);
        await _productCreatePage.ClickCreate();

        await _productListPage.ClickOnProductDetailsLnk(product.Name);

        await Assertions.Expect(_productDetailsPage.GetPageTitleElement()).ToBeVisibleAsync();
        await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToBeVisibleAsync();
        await Assertions.Expect(_productDetailsPage.GetProductNameElement()).ToHaveTextAsync(product.Name);
    }
}
