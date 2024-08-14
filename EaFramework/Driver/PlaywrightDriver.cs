﻿using Microsoft.Playwright;
using EaFramework.Config;

namespace EaFramework.Driver;

public class PlaywrightDriver : IDisposable, IPlaywrightDriver
{
	private readonly AsyncTask<IBrowser> _browser;
	private readonly AsyncTask<IBrowserContext> _browserContext;
	private readonly TestSettings _testSettings;
	private readonly AsyncTask<IPage> _page;
	private readonly AsyncTask<IAPIRequestContext> _apiRequestContext;
	private readonly IPlaywrightDriverInitializer _playwrightDriverInitializaer;
	private bool _isDisposed;

	public PlaywrightDriver(TestSettings testSettings, IPlaywrightDriverInitializer playwrightDriverInitializer)
	{
		_testSettings = testSettings;
		_playwrightDriverInitializaer = playwrightDriverInitializer;

		_browser = new AsyncTask<IBrowser>(InitializePlaywrightAsync);
		_browserContext = new AsyncTask<IBrowserContext>(CreateBrowserContext);
		_page = new AsyncTask<IPage>(CreatePageAsync);
		_apiRequestContext = new AsyncTask<IAPIRequestContext>(CreateApiContext);
	}

	public Task<IPage> Page => _page.Value;

	public Task<IBrowser> Browser => _browser.Value;

	public Task<IBrowserContext> BrowserContext => _browserContext.Value;

	public Task<IAPIRequestContext> ApiRequestContext => _apiRequestContext.Value;

	private async Task<IBrowser> InitializePlaywrightAsync()
	{
		return _testSettings.DriverType switch
		{
			DriverType.Chromium => await _playwrightDriverInitializaer.GetChromiumDriverAsync(_testSettings),
			DriverType.Firefox => await _playwrightDriverInitializaer.GetFirefoxDriverAsync(_testSettings),
			DriverType.Edge => await _playwrightDriverInitializaer.GetEdgeDriverAsync(_testSettings),
			DriverType.Webkit => await _playwrightDriverInitializaer.GetWebkitDriverAsync(_testSettings),
			_ => await _playwrightDriverInitializaer.GetChromeDriverAsync(_testSettings)
		};
	}

	private async Task<IAPIRequestContext> CreateApiContext()
	{
		var playwright = await Playwright.CreateAsync();

		return await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
		{
			BaseURL = _testSettings.ApplicationApiUrl,
			//can be moved to config
			IgnoreHTTPSErrors = true
		});
	}

	private async Task<IBrowserContext> CreateBrowserContext()
	{
		return await (await _browser).NewContextAsync();
	}

	private async Task<IPage> CreatePageAsync()
	{
		return await (await _browserContext).NewPageAsync();
	}

	public void Dispose()
	{
		if (_isDisposed) return;

		if (_browser.IsValueCreated)
			Task.Run(async () =>
			{
				await (await Browser).CloseAsync();
				await (await Browser).DisposeAsync();
			});

		if (_apiRequestContext.IsValueCreated)
			Task.Run(async () =>
			{
				await (await ApiRequestContext).DisposeAsync();
			});

		_isDisposed = true;
	}
}
