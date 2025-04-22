using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Serilog;
using EpamTestAutomationTask.Utilities;
using EpamTestAutomationTask.Pages;

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

        LoginPage loginPage = new LoginPage(_driver);

        loginPage.EnterUsername("ValidUsername");
        loginPage.EnterPassword("ValidPassword");

        loginPage.EnterUsername("");
        loginPage.EnterPassword("");

        loginPage.ClickLoginButton();

        var errorMessage = loginPage.GetErrorMessage();

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

        LoginPage loginPage = new LoginPage(_driver);

        loginPage.EnterUsername("ValidUsername");
        loginPage.EnterPassword("");

        loginPage.ClickLoginButton();

        var errorMessage = loginPage.GetErrorMessage();

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

        LoginPage loginPage = new LoginPage(_driver);

        loginPage.EnterUsername("standard_user");
        loginPage.EnterPassword("secret_sauce");

        loginPage.ClickLoginButton();

        var logoText = loginPage.GetLogo();

        logoText.Should().Be("Swag Labs");

        Log.Information("UC-3 passed on {Browser}", browser);
    }
}

