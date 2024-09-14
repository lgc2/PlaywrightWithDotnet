using EaFramework.Config;
using EaFramework.Driver;
using EaSpecflowTests.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Data;
using ProductAPI.Repository;
using SolidToken.SpecFlow.DependencyInjection;

namespace EaSpecflowTests;

public class Startup
{
	[ScenarioDependencies]
	public static IServiceCollection CreateServices()
	{
		var services = new ServiceCollection();

		var dbPath = GetProjectDbPath("ProductAPI");
		// Connection string as the path of Product.db from ProductAPI folder
		var connectionString = $"Data Source={dbPath}/Product.db";
		// Connect to DB
		services.AddDbContext<ProductDbContext>(option => option.UseSqlite(connectionString));

		services
			.AddSingleton(ConfigReader.ReadConfig())
			.AddScoped<IPlaywrightDriver, PlaywrightDriver>()
			.AddScoped<IPlaywrightDriverInitializer, PlaywrightDriverInitializer>()
			.AddScoped<IProductListPage, ProductListPage>()
			.AddScoped<IProductCreatePage, ProductCreatePage>()
			.AddScoped<IProductDetailsPage, ProductDetailsPage>()
			.AddTransient<IProductRepository, ProductRepository>();

		return services;
	}

	private static string GetProjectDbPath(string projectName)
	{
		// Get the base directory of the currently running appplication
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

		// Traverse up to the solution directory
		var directory = new DirectoryInfo(baseDirectory);
		while (directory != null && !DirectoryContains(directory.FullName, "docker-compose.yml"))
		{
			directory = directory.Parent;
		}
		if (directory == null)
		{
			throw new Exception("Solution directory not found.");
		}

		// Construct the path to the target project's directory and return it
		return Path.Combine(directory.FullName, projectName);
	}

	private static bool DirectoryContains(string directory, string pattern)
	{
		return Directory.GetFiles(directory, pattern).Length > 0;
	}
}
