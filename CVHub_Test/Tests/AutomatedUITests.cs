using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace CVHub_test
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        public AutomatedUITests()
        {
            _driver = new ChromeDriver();
        }
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void LoginWithTestUser_ShouldReturn_SuccessfullLogin()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:44382/");
            _driver.FindElement(By.Id("inputEmail"))
                .SendKeys("test@test.com");
            _driver.FindElement(By.Id("inputPassword"))
                .SendKeys("123456");
            _driver.FindElement(By.Id("submit"))
                .Click();

            Assert.Equal("https://localhost:44382/User/MyPage", _driver.Url);
        }


        //[Fact]
        //public void Create_WhenExecuted_ReturnsNewUser()
        //{
        //    _driver.Navigate()
        //        .GoToUrl("https://localhost:44382/");
        //    _driver.FindElement(By.Id("register"))
        //        .Click();
        //    _driver.
        //}
    }
}