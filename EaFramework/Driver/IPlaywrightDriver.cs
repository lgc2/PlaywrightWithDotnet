using Microsoft.Playwright;

namespace EaFramework.Driver
{
	public interface IPlaywrightDriver
	{
		Task<IBrowser> Browser { get; }
		Task<IBrowserContext> BrowserContext { get; }
		Task<IPage> Page { get; }
		Task<IAPIRequestContext> ApiRequestContext { get; }
		Task<string> TakeScreenshotAsPathAsync(string fileName);
		void Dispose();
	}
}