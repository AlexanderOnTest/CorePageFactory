using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using OpenQA.Selenium;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using System;

namespace Tests
{
    public class Tests
    {
        private IServiceProvider serviceProvider;
        private IWebDriverManager driverManager;
        private IWebDriver driver;


        [OneTimeSetUp]
        public void Setup()
        {
            IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection(true, WebDriverSettings.WebDriverConfiguration);

            serviceProvider = serviceCollection.BuildServiceProvider();
            this.driverManager = serviceProvider.GetService<IWebDriverManager>();
        }

        [Test]
        public void LaunchDriver()
        {
            this.driver = this.driverManager.Get();
            this.driver.Url = "http://www.example.com";
            Assert.AreEqual("Example Domain", driver.Title);
        }

        [TearDown]
        public void TearDown()
        {
            this.driverManager?.Quit();
        }
    }
}