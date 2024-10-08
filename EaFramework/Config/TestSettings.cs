﻿namespace EaFramework.Config;

public class TestSettings
{
	public string[] Args { get; set; }
	public float Timeout { get; set; } = 30.0f;
	public bool Headless { get; set; } = true;
	public bool DevTools { get; set; } = false;
	public float SlowMo { get; set; }
	public DriverType DriverType { get; set; } = DriverType.Chrome;
	public string ApplicationUrl { get; set; }
	public string ApplicationApiUrl { get; set; }
}

public enum DriverType
{
	Chromium,
	Firefox,
	Edge,
	Chrome,
	Webkit
}
