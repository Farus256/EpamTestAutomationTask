using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using FluentAssertions;
using Serilog;
using EpamTestAutomationTask.Utilities;

namespace EpamTestAutomationTask.Tests;

[TestClass]
public class LoginTests : BaseTest
{
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
}

