using System;
using AlexanderOnTest.NetCoreWebDriverFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using Scrutor;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class PageTests
    {
        private string TestPageTitle => "AlexanderOnTest - PageFactory Test Page";

        private IWebDriver Driver { get; set; }

        private IWebDriverFactory WebDriverFactory { get; set; }

        private IWebDriverManager DriverManager { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        private TestPage TestPage { get; set; }

        [Test]
        public void ExpectedTitleIsReturnedCorrectly()
        {
            TestPage.GetExpectedPageTitle().Should().Be(TestPageTitle);
        }
        
        [Test]
        public void TitleIsReturnedCorrectly()
        {
            TestPage.GetActualPageTitle().Should().Be(TestPageTitle);
        }
        
        [Test]
        public void UriStringIsReturnedCorrectly()
        {
            TestPage.GetActualUri().Should().Be(TestSettings.TestPageUriString);
        }
        
        [Test]
        public void ExpectedUriStringIsReturnedCorrectly()
        {
            TestPage.GetActualUri().Should().Be(TestPage.GetExpectedUri());
        }

        #region SetUpTearDown

        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServiceProvider = ConfigurationModule.GetServiceProvider(true);
            DriverManager = ServiceProvider.GetRequiredService<IWebDriverManager>();
            Driver = ServiceProvider.GetRequiredService<IWebDriver>();
            TestPage = ServiceProvider.GetRequiredService<TestPage>();
            Driver.Url = TestSettings.TestPageUriString;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DriverManager.Quit();
            DriverManager?.Dispose();
            WebDriverFactory?.Dispose();
        }

        #endregion

    }
}