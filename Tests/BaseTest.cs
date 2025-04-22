using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using EpamTestAutomationTask.Utilities;
using EpamTestAutomationTask.Pages;

namespace EpamTestAutomationTask.Tests
{
    public class BaseTest
    {
        protected IWebDriver _driver;
        protected const string BaseUrl = "https://www.saucedemo.com/";

        // Login info
        protected const string ValidUsername = "standard_user";
        protected const string ValidPassword = "secret_sauce";

  

        // Stores current test info
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            // Initialize logger
            Logger.InitializeLogger();
            Logger.Message("Test started");
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
                    Logger.Error(ex, "Error closing driver");
                }
                Logger.Message("Test ended");
                Logger.CloseLogger();
            }
        }

        protected BrowserType GetBrowserType(string browser)
        {
            if (browser.Equals("firefox", StringComparison.OrdinalIgnoreCase))
                return BrowserType.Firefox;
            if (browser.Equals("edge", StringComparison.OrdinalIgnoreCase))
                return BrowserType.Edge;
            throw new ArgumentException("Unsupported browser type");
        }
    }
}
