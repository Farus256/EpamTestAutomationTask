using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EpamTestAutomationTask.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement UsernameField => _driver.FindElement(By.CssSelector("[data-test='username']"));
        private IWebElement PasswordField => _driver.FindElement(By.CssSelector("[data-test='password']"));
        private IWebElement LoginButton => _driver.FindElement(By.CssSelector("[data-test='login-button']"));
       

        public void EnterUsername(string username)
        {
            UsernameField.Click();
            UsernameField.SendKeys(Keys.Control + "a");
            UsernameField.SendKeys(Keys.Backspace);
            UsernameField.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordField.Click();
            PasswordField.SendKeys(Keys.Control + "a");
            PasswordField.SendKeys(Keys.Backspace);
            PasswordField.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            LoginButton.Click();
        }

        public string GetErrorMessage()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            var errorElement = wait.Until(_driver => _driver.FindElement(By.CssSelector("[data-test='error']")));
            return errorElement.Text;
        }

        public string GetLogo()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            var logoElement = wait.Until(_driver => _driver.FindElement(By.CssSelector(".app_logo")));
            return logoElement.Text;
        }

    }
}
