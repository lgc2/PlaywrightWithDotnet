using Microsoft.Playwright;

namespace EaFramework.Driver
{
	public interface IPlaywrightDriver
	{
		Task<IBrowser> Browser { get; }
		Task<IBrowserContext> BrowserContext { get; }
		Task<IPage> Page { get; }
		void Dispose();
	}
}