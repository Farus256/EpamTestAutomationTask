using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using FluentAssertions;
using Serilog;
using EpamTestAutomationTask.Utilities;

namespace EpamTestAutomationTask;

[TestClass]
public class LoginTests
{
    private IWebDriver _driver;
    private const string BaseUrl = "https://www.saucedemo.com/";

    // Login info
    private const string ValidUsername = "standard_user";
    private const string ValidPassword = "secret_sauce";

    // Stores current test info
    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void SetUp()
    {
        // Initialize logger
        Logger.InitializeLogger();
        Log.Information("Test started");
    }

    [TestCleanup]
    public void EndTest()
    {
        try
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed && _driver != null)
            {
                Logger.SaveScreenshot((ITakesScreenshot)_driver);
            }
        }
        finally
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
                _driver = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error closing driver");
            }
            Log.Information("Test ended");
            Logger.CloseLogger();
        }
    }

    [TestMethod]
    [DataRow("firefox")]
    [DataRow("edge")]
    public void UC1_EmptyCredentials_ShouldShowUsernameError(string browser)
    {
        Log.Information("Starting UC-1 on {Browser}", browser);
        _driver = WebDriverFactory.CreateDriver(GetBrowserType(browser));
        _driver.Navigate().GoToUrl(BaseUrl);

        var usernameField = _driver.FindElement(By.CssSelector("[data-test='username']"));
        var passwordField = _driver.FindElement(By.CssSelector("[data-test='password']"));
        var loginButton = _driver.FindElement(By.CssSelector("[data-test='login-button']"));

        usernameField.SendKeys(ValidUsername);
        passwordField.SendKeys(ValidPassword);

        usernameField.SendKeys(Keys.Control + "a");
        usernameField.SendKeys(Keys.Delete);

        passwordField.SendKeys(Keys.Control + "a");
        passwordField.SendKeys(Keys.Delete);

        loginButton.Click();

        var errorMessage = _driver.FindElement(By.CssSelector("[data-test='error']")).Text;
        errorMessage.Should().Contain("Username is required");
        Log.Information("UC-1 passed on {Browser}", browser);
    }

    [TestMethod]
    [DataRow("firefox")]
    [DataRow("edge")]
    public void UC2_MissingPassword_ShouldShowPasswordError(string browser)
    {
        Log.Information("Starting UC-2 on {Browser}", browser);
        _driver = WebDriverFactory.CreateDriver(GetBrowserType(browser));
        _driver.Navigate().GoToUrl(BaseUrl);

        var usernameField = _driver.FindElement(By.CssSelector("[data-test='username']"));
        var passwordField = _driver.FindElement(By.CssSelector("[data-test='password']"));
        var loginButton = _driver.FindElement(By.CssSelector("[data-test='login-button']"));

        usernameField.SendKeys(ValidUsername);
        passwordField.SendKeys(ValidPassword);

        passwordField.SendKeys(Keys.Control + "a");
        passwordField.SendKeys(Keys.Delete);

        loginButton.Click();

        var errorMessage = _driver.FindElement(By.CssSelector("[data-test='error']")).Text;
        errorMessage.Should().Contain("Password is required");
        
        Log.Information("UC-2 passed on {Browser}", browser);
    }

    [TestMethod]
    [DataRow("firefox")]
    [DataRow("edge")]
    public void UC3_ValidCredentials_ShouldLoginSuccessfully(string browser)
    {
        Log.Information("Starting UC-3 on {Browser}", browser);
        _driver = WebDriverFactory.CreateDriver(GetBrowserType(browser));
        _driver.Navigate().GoToUrl(BaseUrl);

        var usernameField = _driver.FindElement(By.CssSelector("[data-test='username']"));
        var passwordField = _driver.FindElement(By.CssSelector("[data-test='password']"));
        
        usernameField.SendKeys(ValidUsername);
        passwordField.SendKeys(ValidPassword);

        var loginButton = _driver.FindElement(By.CssSelector("[data-test='login-button']"));
        loginButton.Click();

        var logoElement = _driver.FindElement(By.CssSelector(".app_logo"));
        var logoText = logoElement.Text;
        logoText.Should().Be("Swag Labs");

        Log.Information("UC-3 passed on {Browser}", browser);
    }

    private BrowserType GetBrowserType(string browser)
    {
        if (browser.Equals("firefox", StringComparison.OrdinalIgnoreCase))
            return BrowserType.Firefox;
        if (browser.Equals("edge", StringComparison.OrdinalIgnoreCase))
            return BrowserType.Edge;
        throw new ArgumentException("Unsupported browser type");
    }
}

