using EaFramework.Driver;
using EaSpecflowTests.Models;
using Microsoft.Playwright;

namespace EaSpecflowTests.Steps;

[Binding]
public class ReusableSteps
{
	private readonly IPlaywrightDriver _playwrightDriver;

	public ReusableSteps(IPlaywrightDriver playwrightDriver)
	{
		_playwrightDriver = playwrightDriver;
	}

	[Given(@"I ensure ""([^""]*)"" data is cleaned up if it already exists")]
	public async Task GivenIEnsureDataIsCleanedUpIfItAlreadyExists(string productName)
	{
		await (await _playwrightDriver.ApiRequestContext).DeleteAsync("Product/DeleteByName",
		new APIRequestContextOptions()
		{
			Params = new Dictionary<string, object>
			{
					{ "name", $"{productName}" }
			}
		});
	}
}
