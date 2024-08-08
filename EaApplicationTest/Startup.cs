using EaApplicationTest.Fixture;
using EaApplicationTest.Pages;
using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Extensions.DependencyInjection;

namespace EaApplicationTest;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services
			.AddSingleton(ConfigReader.ReadConfig())
			.AddScoped<IPlaywrightDriver, PlaywrightDriver>()
			.AddScoped<IPlaywrightDriverInitializer, PlaywrightDriverInitializer>()
			.AddScoped<IProductListPage, ProductListPage>()
			.AddScoped<IProductCreatePage, ProductCreatePage>()
			.AddScoped<IProductDetailsPage, ProductDetailsPage>()
			.AddScoped<ITestFixtureBase, TestFixtureBase>();
	}
}
