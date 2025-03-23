using OpenQA.Selenium;
using Serilog;

namespace EpamTestAutomationTask.Utilities;

public static class Logger
{   
    private static readonly string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

    public static void InitializeLogger()
    {
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }

        var logConfig = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug();

        logConfig = logConfig.WriteTo.File(
            Path.Combine(logPath, "log.txt"),
            rollingInterval: RollingInterval.Day,
            shared: true,
            flushToDiskInterval: TimeSpan.FromSeconds(1)
        );

        Log.Logger = logConfig.CreateLogger();
    }

    public static void CloseLogger()
    {
        Log.CloseAndFlush();
    }

    public static void SaveScreenshot(ITakesScreenshot driver)
    {
        try
        {
            var screenshot = driver.GetScreenshot();
            var fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var filePath = Path.Combine(logPath, fileName);
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
            Log.Information("Screenshot saved: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error saving screenshot");
        }
    }
}
