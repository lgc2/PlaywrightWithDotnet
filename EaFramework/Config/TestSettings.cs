namespace EaFramework.Config;

public class TestSettings
{
	public string[] Args { get; set; }
	public float Timeout { get; set; } = 30.0f;
	public bool Headless { get; set; } = true;
	public bool DevTools { get; set; } = false;
	public int SlowMo { get; set; }
	public DriverType DriverType { get; set; } = DriverType.Chrome;
}

public enum DriverType
{
	Chromium,
	Firefox,
	Edge,
	Chrome,
	Webkit
}
