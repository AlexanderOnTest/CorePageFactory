using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NewNetPageFactory.SystemTests.TestPageControllers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NewNetPageFactory.SystemTests
{
    public class PageTests
    {
        private string TestPageTitle => "AlexanderOnTest - PageFactory Test Page";

        private IServiceProvider ServiceProvider { get; set; }

        private IWebDriver Driver => ServiceProvider.GetRequiredService<IWebDriver>();

        private IWebDriverManager DriverManager => ServiceProvider.GetRequiredService<IWebDriverManager>();

        private TestPage TestPage => ServiceProvider.GetRequiredService<TestPage>();

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
            Driver.Url = TestSettings.TestPageUriString;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DriverManager?.Quit();
            DriverManager?.Dispose();
        }

        #endregion

    }
}