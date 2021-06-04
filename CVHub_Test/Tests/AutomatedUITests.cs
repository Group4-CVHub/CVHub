﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace CVHub_Test.AutomatedUITests
{
    [TestClass]
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        public AutomatedUITests()
        {
            _driver = new ChromeDriver();
        }

        [TestMethod]
        public void Login_With_Test_User_Should_Return_Successfull_Login()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:44382/");
            _driver.FindElement(By.Id("inputEmail"))
                .SendKeys("test@test.com");
            _driver.FindElement(By.Id("inputPassword"))
                .SendKeys("123456");
            _driver.FindElement(By.Id("submit"))
                .Click();

            Assert.AreEqual("https://localhost:44382/User/MyPage", _driver.Url);
        }

        [TestMethod]
        public void Create_New_User_When_Executed_Should_Return_New_User()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:44382/");
            _driver.FindElement(By.Id("register"))
                .Click();
            _driver.FindElement(By.Id("firstname"))
                .SendKeys("Test");
            _driver.FindElement(By.Id("lastname"))
                .SendKeys("Test");
            _driver.FindElement(By.Id("country"))
                .SendKeys("Test");
            _driver.FindElement(By.Id("state"))
                .SendKeys("Test");
            _driver.FindElement(By.Id("city"))
                .SendKeys("Test");
            _driver.FindElement(By.Id("LinkedIn"))
                .SendKeys("www.test/linkedin.com");
            _driver.FindElement(By.Id("email"))
                .SendKeys("test2@test2.com");
            _driver.FindElement(By.Id("phone-number"))
                .SendKeys("1234567891");
            _driver.FindElement(By.Id("password"))
                .SendKeys("123456");
            _driver.FindElement(By.Id("createAccount"))
                .Click();

            Assert.AreEqual("localhost:44382/User/MyPage", _driver.Url);
            Assert.AreEqual("Test", _driver.FindElement(By.Id("firstName")).Text);
            Assert.AreEqual("Test", _driver.FindElement(By.Id("lastName")).Text);
        }

        [TestMethod]
        public void Try_To_Go_To_TemplateForm1_When_Not_Logged_In_Should_Return_Redirect_Url()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:44382/");
            _driver.FindElement(By.Id("templates"))
                .Click();
            _driver.FindElement(By.Id("choose"))
                .Click();

            Assert.AreEqual("https://localhost:44382/Validation/SignIn?ReturnUrl=%2FTemplate%2FTemplateForm1", _driver.Url);
        }

        [TestCleanup]
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
    
