using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using EaFramework.Config;
using EaFramework.Driver;
using Microsoft.Playwright;
using TechTalk.SpecFlow.Bindings;

namespace EaSpecflowTests.Hooks;

[Binding]
public class Hook
{
	private readonly TestSettings _testSettings;
	private readonly FeatureContext _featureContext;
	private readonly ScenarioContext _scenarioContext;
	private readonly Task<IPage> _page;
	private static ExtentReports? _extentReports;
	private ExtentTest _scenario;

	public Hook(IPlaywrightDriver playwrightDriver, TestSettings testSettings, FeatureContext featureContext, ScenarioContext scenarioContext)
	{
		_testSettings = testSettings;
		_featureContext = featureContext;
		_scenarioContext = scenarioContext;
		_page = playwrightDriver.Page;
	}

	[BeforeTestRun]
	public static void InitializeExtentReports()
	{
		_extentReports = new ExtentReports();
		var spark = new ExtentSparkReporter("TestReports.html");
		_extentReports.AttachReporter(spark);
	}

	[BeforeScenario]
	public async Task BeforeScenario()
	{
		var feature = _extentReports.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
		_scenario = feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);

		await (await _page).GotoAsync(_testSettings.ApplicationUrl);
	}

	[AfterStep]
	public void AfterStep()
	{
		if (_scenarioContext.TestError == null)
		{
			switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
			{
				case StepDefinitionType.Given:
					_scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
					break;
				case StepDefinitionType.When:
					_scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
					break;
				case StepDefinitionType.Then:
					_scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		else
		{
			switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
			{
				case StepDefinitionType.Given:
					_scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
					break;
				case StepDefinitionType.When:
					_scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
					break;
				case StepDefinitionType.Then:
					_scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	[AfterTestRun]
	public static void TearDownReport() => _extentReports?.Flush();
}
