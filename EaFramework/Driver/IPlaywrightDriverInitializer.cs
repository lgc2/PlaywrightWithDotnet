using Microsoft.Playwright;
using EaFramework.Config;

namespace EaFramework.Driver
{
	public interface IPlaywrightDriverInitializer
	{
		Task<IBrowser> GetChromeDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetChromiumDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetEdgeDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetFirefoxDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetWebkitDriverAsync(TestSettings testSettings);
	}
}